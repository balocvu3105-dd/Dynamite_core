# Dynamite Core — Distributed Discord Management & Automation Platform

[![.NET 8](https://img.shields.io/badge/.NET%208.0-C%23-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-REST%20API-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![React 19](https://img.shields.io/badge/React%2019-Vite%20+%20TS-61DAFB?logo=react&logoColor=black)](https://react.dev/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16%20Debian-336791?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-Multi--Container-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20DDD-success)](https://github.com/balocvu3105-dd/Dynamite_Core)
[![Tests](https://img.shields.io/badge/Unit%20Tests-44%20Passing-brightgreen)](https://github.com/balocvu3105-dd/Dynamite_Core)

**Dynamite Core** is a modular backend ecosystem designed to manage and automate high-traffic Discord communities. Built on **.NET 8**, **ASP.NET Core**, and **React 19**, the project demonstrates how to structure a complex, multi-service backend using **Clean Architecture** and **Domain-Driven Design (DDD)** while addressing concurrency, state synchronization, and background job processing in containerized environments.

---

## 🏛️ System Overview

Building a resilient communication platform on top of the Discord Gateway involves handling high-frequency, asynchronous WebSocket event streams while servicing real-time web requests. This requires solving fundamental backend engineering problems:

* **Scope Management:** Preventing thread-safety issues between long-lived singleton gateway connections and scoped database transactions.
* **Latency & Rate Limiting:** Minimizing database I/O during high-volume chat events via in-memory sliding windows.
* **Idempotent Background Jobs:** Ensuring scheduled tasks (giveaways, temporary voice channels, leaderboards) survive container restarts without duplicate execution.
* **Stateless Security:** Securing Single-Page Applications (SPAs) communicating with REST endpoints via strict OAuth2 authorization code flows.

To isolate these concerns and prevent monolithic entanglement, the ecosystem is partitioned into dedicated runtime applications and domain modules.

---

## 📐 Architecture & Data Flow

```mermaid
graph TD
    subgraph Client Layer ["Client Layer"]
        SPA["💻 React 19 + Vite Dashboard<br/>(TailwindCSS / TypeScript)"]
        DiscordClient["📱 Discord Client / Community Users<br/>(Gateway Events & Commands)"]
    end

    subgraph Presentation / Host Layer ["Presentation / Host Layer (.NET 8)"]
        API["🌐 Dynamite.API (ASP.NET Core)<br/>• OAuth2 Code Exchange & JWT Engine<br/>• ErrorHandlingMiddleware<br/>• REST Controllers & DTOs"]
        Bot["🤖 Dynamite.Bot (IHostedService)<br/>• DiscordSocketClient Gateway Loop<br/>• Event Dispatcher & Scope Bridging<br/>• Background Scheduling Workers"]
    end

    subgraph Application & Module Layer ["Application & Domain Modules Layer"]
        App["📦 Dynamite.Application<br/>• Service Contracts & Interfaces<br/>• CQRS Handlers & DTO Mapping"]
        Modules["🧩 Dynamite.Modules.* (6+ Feature Projects)<br/>• Moderation | Security | Economy<br/>• Voice | Giveaway | Ticket | Logging"]
    end

    subgraph Core Domain Layer ["Core Domain Layer"]
        Core["💎 Dynamite.Core<br/>• Domain Entities & Aggregates<br/>• Value Objects & Domain Exceptions<br/>• Repository & UnitOfWork Contracts"]
    end

    subgraph Infrastructure Layer ["Infrastructure & Storage"]
        Infra["⚙️ Dynamite.Infrastructure<br/>• EF Core 8 DbContext & Migrations<br/>• Npgsql / PostgreSQL Repositories<br/>• Discord REST Client Integrations"]
        PG[("🐘 PostgreSQL 16 (Debian)<br/>• ACID Transactions<br/>• Relational Domain Storage")]
    end

    SPA <-->|REST / JSON (JWT + Cookie)| API
    DiscordClient <-->|WebSocket Gateway / API v10| Bot
    API --> App
    Bot --> App
    App --> Modules
    Modules --> Core
    App --> Core
    Infra --> Core
    API --> Infra
    Bot --> Infra
    Infra <-->|EF Core / SQL| PG
```

### Core Architectural Principles

1. **Clean Architecture & Dependency Inversion (`DIP`):**
   * Business rules exist strictly inside `Dynamite.Core` (domain models and repository interfaces) and `Dynamite.Application` (service orchestration).
   * `Dynamite.Infrastructure` implements data persistence (`AppDbContext`, `IRepository<T>`, `IUnitOfWork`) and external integrations without leaking concrete ORM dependencies upstream.
   * `Dynamite.API` and `Dynamite.Bot` serve as thin presentation hosts that resolve application services via dependency injection.

2. **Modular Monolith Decomposition:**
   * Instead of grouping all business features into one project, domain capabilities are separated into distinct class libraries (`Dynamite.Modules.Moderation`, `Dynamite.Modules.Security`, `Dynamite.Modules.Economy`, etc.).
   * Each module defines isolated feature logic, preventing circular dependencies and allowing individual feature boundaries to be tested independently.

3. **Repository & Unit of Work Pattern:**
   * Multi-step domain mutations (e.g., deducting economy currency, generating inventory items, and creating audit entries) are coordinated through `IUnitOfWork`. Changes are staged across repositories and committed inside a single database transaction (`SaveChangesAsync`), ensuring data consistency.

---

## 🛠️ Key Engineering Decisions & Trade-Offs

### 1. Bridging Singleton Gateway Loops and Scoped Database Contexts
* **Problem:** `DiscordSocketClient` runs as a long-lived `Singleton` service that fires concurrent asynchronous event handlers across pool threads. Entity Framework Core’s `DbContext` is `Scoped` and not thread-safe. Injecting a shared or singleton `DbContext` directly into gateway event handlers leads to `ConcurrentContextUsageException` or connection starvation under heavy server loads.
* **Design Decision:** Every incoming gateway event dynamically creates an explicit service scope (`IServiceScopeFactory.CreateScope()`). Scoped dependencies (`IUnitOfWork`, repositories, domain services) are resolved and executed exclusively within that execution boundary before being disposed cleanly.

```csharp
// Pattern used across gateway event consumers (e.g., BotHostedService)
private async Task HandleMessageReceivedAsync(SocketMessage socketMessage)
{
    if (socketMessage is not SocketUserMessage message || message.Author.IsBot)
        return;

    // Create an isolated scope per gateway event to ensure thread-safe DbContext usage
    using var scope = _services.CreateScope();
    var securityService = scope.ServiceProvider.GetRequiredService<ISecurityService>();
    
    await securityService.EvaluateMessageAsync(message);
}
```

### 2. In-Memory Sliding Windows vs. Database Query Overhead
* **Problem:** Querying PostgreSQL on every chat message across multiple servers to check for spam signatures or raid thresholds introduces significant I/O latency and rapidly exhausts database connection pools.
* **Design Decision:** `Dynamite.Modules.Security` implements thread-safe, in-memory sliding windows using `ConcurrentDictionary` and timestamp buckets. Rate-limit checks and message similarity evaluations happen entirely in RAM. When violation thresholds are breached within a moving time window, an autonomous escalation ladder progressive applies penalties (Warning → Timeout → Kick → Ban) and dispatches asynchronous audit logs to PostgreSQL without blocking the main event loop.

### 3. Idempotent Background Scheduling via `BackgroundService`
* **Problem:** In-memory timers (`Task.Delay` or `System.Threading.Timer`) used for scheduled events (giveaway draws, temporary voice channel cleanup, leaderboard updates) lose their state when containers are restarted or redeployed during CI/CD pipelines.
* **Design Decision:** Implemented dedicated `BackgroundService` workers that periodically poll PostgreSQL (`SELECT ... WHERE EndTime <= @Now AND IsCompleted = false`). To prevent race conditions or duplicate execution when multiple worker instances run or during container restarts, state transitions (`IsCompleted = true`) are wrapped inside atomic database transactions before triggering external side effects.

### 4. Stateless API & OAuth2 Authorization Code Flow (`React 19` + `.NET 8`)
* **Problem:** A decoupled React Single-Page Application requires secure authentication with Discord without exposing client secrets or storing vulnerable OAuth access tokens in browser local storage.
* **Design Decision:** Implemented an exact-match OAuth2 Authorization Code flow where the React SPA passes the authorization code (`redirect_uri`) to `Dynamite.API`. The backend validates CSRF protection via HTTP-only state cookies, exchanges the code with Discord v10 REST APIs, and issues a stateless JWT access token (`X-Discord-Token`) for API endpoints. Long-lived refresh tokens are stored as `SHA-256` hashes in PostgreSQL (`RefreshTokens` table), enabling secure token rotation and instant revocation upon logout.

### 5. Container Storage Checkpoint Resilience & Graceful Shutdown
* **Problem:** Early iterations running `postgres:alpine` (`musl libc`) under heavy database checkpoint write loads on certain Linux VPS kernel filesystems (`overlayfs` / `ext4`) triggered `ENODATA` or `EUCLEAN` kernel errors, forcing the PostgreSQL checkpointer into a `PANIC` crash loop.
* **Design Decision:** Migrated database container infrastructure to `postgres:16` (`Debian glibc`) for full POSIX filesystem flushing compliance. Additionally, `.NET 8` host services (`Dynamite.Bot` and `Dynamite.API`) implement explicit shutdown hooks (`AppDomain.CurrentDomain.UnhandledException`, `clean_shutdown.flag`, and Serilog `Log.CloseAndFlush()`) to ensure buffered logs and transient states are flushed cleanly during container termination (`SIGTERM`).

---

## 📦 Project Structure (`src/`)

| Project | Layer | Role & Responsibilities |
| :--- | :--- | :--- |
| **`Dynamite.Core`** | **Core Domain** | Domain Entities (`GuildConfig`, `UserAccount`, `ModerationAction`), Enums, Domain Exceptions, and Data Access Contracts (`IRepository<T>`, `IUnitOfWork`). |
| **`Dynamite.Application`** | **Application** | Use Case Contracts (`IAuthService`, `ISecurityService`), DTOs (`AuthResponse`, `DiscordUserDto`), and Business Workflow Orchestration. |
| **`Dynamite.Infrastructure`** | **Infrastructure** | `AppDbContext` (EF Core 8), Entity Configurations (`IEntityTypeConfiguration<T>`), Npgsql Repositories, and Discord REST Clients. |
| **`Dynamite.Shared`** | **Shared Constants** | Shared cross-layer models, utilities, and configuration schemas. |
| **`Dynamite.API`** | **Presentation / Edge** | ASP.NET Core REST API Endpoints, JWT Token Generator, `ErrorHandlingMiddleware`, CORS policies, Rate Limiting, and Swagger UI. |
| **`Dynamite.Bot`** | **Presentation / Worker** | `DiscordSocketClient` Gateway Host, Slash Command Execution Engine (`CommandService`), Gateway Event Handlers, and Background Schedulers. |
| **`Dynamite.Migrator`** | **DevOps / Tooling** | Autonomous EF Core Migration Console Application. Executed inside container startup pipelines (`docker compose up`) before API/Bot boot. |
| **`Dynamite.Modules.*`** | **Feature Modules** | Dedicated domain assemblies (`Moderation`, `Security`, `Economy`, `Voice`, `Giveaway`, `Ticket`, `Logging`, `RoleManagement`, `Setup`, `Welcome`). |
| **`Dynamite.Tests`** | **Quality Assurance** | Automated Unit & Integration Tests using `xUnit` and `Moq` verifying domain calculation formulas, moderation rules, and service boundaries. |
| **`dynamite-dashboard`** | **Frontend SPA** | React 19 + TypeScript + Vite + TailwindCSS Single-Page Application with typed API clients and responsive dashboard layouts. |

---

## 🧪 Quality Assurance & Testing

The codebase includes an automated suite of tests using `xUnit` and `Moq` to verify core business logic, economy mathematical models (interest calculations, tax deductions), and moderation enforcement boundaries:

```bash
# Execute unit test suite across all modules
dotnet test src/Dynamite.Tests/Dynamite.Tests.csproj -v minimal

# Test Execution Summary:
# Passed!  - Failed: 0, Passed: 44, Skipped: 0, Total: 44, Duration: ~1.0 s
```

---

## 🐳 Running Locally & Deployment (`Docker Compose`)

The entire platform can be launched cleanly using **Docker Compose**, which provisions PostgreSQL, executes schema migrations autonomously (`dynamite_migrator`), and boots the backend API, Bot, and React dashboard.

### Prerequisites
* [Docker & Docker Compose](https://www.docker.com/) installed
* [Discord Bot Application](https://discord.com/developers/applications) with Gateway Intents enabled (`GUILD_MEMBERS`, `MESSAGE_CONTENT`)

### Quick Start

```bash
# 1. Clone the repository
git clone https://github.com/balocvu3105-dd/Dynamite_Core.git
cd Dynamite_Core

# 2. Configure environment variables (Copy from example template)
cp .env.example .env
# Edit .env with your Discord Bot Token, Client ID, Client Secret, and Database credentials

# 3. Build and run all services in detached mode
docker compose up --build -d

# 4. Check container health status and logs
docker ps
docker logs -f dynamite_api
```

### Local .NET Development (Without Docker for Backend)

If developing directly on the host machine using the `.NET CLI`:

```bash
# 1. Start a local PostgreSQL instance (or use Docker for database only)
docker run --name dynamite_db -e POSTGRES_PASSWORD=secret -p 5432:5432 -d postgres:16

# 2. Apply database migrations
dotnet run --project src/Dynamite.Migrator/Dynamite.Migrator.csproj

# 3. Run the REST API and Bot concurrently in separate terminals
dotnet run --project src/Dynamite.API/Dynamite.API.csproj
dotnet run --project src/Dynamite.Bot/Dynamite.Bot.csproj
```

---

## 👨‍💻 Author & Contact

**Bá Lộc Vũ (DynamiteV)**
* **Focus Areas:** C# / .NET 8 Backend Engineering, Distributed Systems, Clean Architecture, Database Optimization (PostgreSQL / EF Core), and DevOps Automation.
* **GitHub Profile:** [github.com/balocvu3105-dd](https://github.com/balocvu3105-dd)
* **Repository:** [github.com/balocvu3105-dd/Dynamite_Core](https://github.com/balocvu3105-dd/Dynamite_Core)
