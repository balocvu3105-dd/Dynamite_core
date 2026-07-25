# Dynamite Core — Distributed Management & Automation Platform

[![.NET 8](https://img.shields.io/badge/.NET%208.0-C%23-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-REST%20API-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![SignalR](https://img.shields.io/badge/SignalR-WebSockets-0078D4?logo=microsoft&logoColor=white)](https://dotnet.microsoft.com/)
[![React 19](https://img.shields.io/badge/React%2019-Vite%20+%20TS-61DAFB?logo=react&logoColor=black)](https://react.dev/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16%20Debian-336791?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-Multi--Container-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)

**Dynamite Core** is a modular, high-performance backend ecosystem designed to manage, automate, and synchronize high-traffic community servers. 

Built on **.NET 8**, **ASP.NET Core**, and **React 19**, this project serves as a comprehensive **Backend / Fullstack Portfolio Piece**. It demonstrates advanced system design, clean architecture, and modern engineering practices for solving real-world concurrency, distributed state, and fault-tolerance problems.

---

## 🏛️ System Overview

Building a resilient communication platform involves handling high-frequency asynchronous event streams while simultaneously servicing real-time web requests. This project addresses several core backend engineering challenges:

* **Scope & Lifetime Management:** Safely bridging long-lived Singleton event listeners (like WebSockets) with Scoped database transactions (Entity Framework Core) to prevent thread-safety exceptions.
* **Real-time Synchronization:** Utilizing **SignalR** to establish seamless, bidirectional communication between the Dashboard, API, and worker nodes. State changes on the web reflect instantly across all distributed workers without polling.
* **Graceful Degradation & Fault Tolerance:** Implementing the **Circuit Breaker** pattern. If a 3rd party API (or permission system) fails repeatedly, the system autonomously "trips the breaker," disabling the faulty module to prevent cascading failures, and pushes a real-time alert to the web dashboard.
* **Idempotent Background Jobs:** Ensuring scheduled tasks (giveaways, temporary voice channels) survive container restarts without duplicate execution through transactional database states.
* **Stateless Security:** Securing Single-Page Applications (SPAs) communicating with REST endpoints via strict OAuth2 authorization code flows and stateless JWTs.

---

## 📐 Architecture & Data Flow

```mermaid
graph TD
    subgraph Client Layer ["Client Layer"]
        SPA["💻 React 19 Dashboard<br/>(TailwindCSS / TypeScript)"]
        DiscordClient["📱 Discord Gateway<br/>(Events & Commands)"]
    end

    subgraph Presentation / Host Layer ["Presentation / Host Layer (.NET 8)"]
        API["🌐 Dynamite.API (ASP.NET Core)<br/>• OAuth2 & JWT Auth<br/>• REST Controllers<br/>• SignalR SyncHub"]
        Bot["🤖 Dynamite.Bot (IHostedService)<br/>• Gateway Connection<br/>• Event Dispatcher<br/>• SignalR Sync Client"]
    end

    subgraph Application & Module Layer ["Application & Domain Modules"]
        App["📦 Dynamite.Application<br/>• CircuitBreakerService<br/>• CQRS Handlers & DTOs"]
        Modules["🧩 Feature Modules<br/>• Moderation | Security<br/>• Voice | Giveaway | Logging"]
    end

    subgraph Core Domain Layer ["Core Domain Layer"]
        Core["💎 Dynamite.Core<br/>• Domain Entities<br/>• Repository Contracts<br/>• Custom Exceptions"]
    end

    subgraph Infrastructure Layer ["Infrastructure & Storage"]
        Infra["⚙️ Dynamite.Infrastructure<br/>• EF Core 8 DbContext<br/>• PostgreSQL Repositories"]
        PG[("🐘 PostgreSQL 16<br/>• ACID Transactions")]
    end

    SPA <-->|REST & SignalR (WebSockets)| API
    DiscordClient <-->|WebSocket Gateway| Bot
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

1. **Clean Architecture & Domain-Driven Design (DDD):**
   * Business rules exist strictly inside `Dynamite.Core` (domain models) and `Dynamite.Application` (service orchestration).
   * `Dynamite.Infrastructure` implements data persistence (`AppDbContext`, `IRepository<T>`, `IUnitOfWork`) without leaking concrete ORM dependencies upstream.

2. **Modular Monolith Decomposition:**
   * Domain capabilities are separated into distinct class libraries (`Moderation`, `Security`, `Logging`, etc.), preventing circular dependencies and enabling independent testing.

3. **Repository & Unit of Work Pattern:**
   * Multi-step domain mutations are coordinated through `IUnitOfWork` and committed inside a single database transaction (`SaveChangesAsync`), ensuring data consistency.

---

## 🛠️ Key Engineering Implementations

### 1. Circuit Breaker Pattern & Real-Time Alerts
* **Problem:** When an external dependency or permission scope fails (e.g., the worker is denied access to perform an action), repeatedly attempting the action wastes compute resources, triggers rate limits, and pollutes logs.
* **Solution:** Implemented `CircuitBreakerService`. It tracks module-specific failure rates. Upon reaching a threshold (e.g., 3 consecutive failures), it "trips", marking the module as faulted in the database (`ModuleFault`). It then fires a **SignalR event** to the React Dashboard, instantly alerting the administrator via a Toast Notification without requiring a page refresh.

### 2. Bridging Singleton Gateway Loops and Scoped Database Contexts
* **Problem:** Event listeners run as long-lived `Singleton` services. Entity Framework Core’s `DbContext` is `Scoped` and not thread-safe.
* **Solution:** Every incoming event dynamically creates an explicit service scope (`IServiceScopeFactory.CreateScope()`). Scoped dependencies (`IUnitOfWork`, repositories) are resolved and executed exclusively within that boundary before being cleanly disposed.

### 3. In-Memory Sliding Windows vs. Database Query Overhead
* **Problem:** Querying PostgreSQL on every high-frequency chat message to check for spam or raid thresholds introduces immense I/O latency.
* **Solution:** Thread-safe, in-memory sliding windows using `ConcurrentDictionary` and timestamp buckets. Rate-limit checks happen entirely in RAM. When thresholds are breached, an autonomous escalation ladder (Warning → Timeout → Kick → Ban) is applied, and audits are persisted asynchronously.

### 4. Stateless API & OAuth2 Authorization Code Flow
* **Problem:** A decoupled React SPA requires secure authentication without exposing client secrets or storing vulnerable tokens in local storage.
* **Solution:** The backend validates CSRF protection via HTTP-only state cookies, exchanges codes with REST APIs, and issues stateless JWT access tokens. Long-lived refresh tokens are stored securely in PostgreSQL as `SHA-256` hashes, enabling token rotation and instant revocation.

---

## 📦 Project Structure (`src/`)

| Project | Role & Responsibilities |
| :--- | :--- |
| **`Dynamite.Core`** | Domain Entities, Enums, Exceptions, and Data Access Contracts (`IRepository<T>`, `IUnitOfWork`). |
| **`Dynamite.Application`** | Use Case Contracts, DTOs, Circuit Breaker logic, and Workflow Orchestration. |
| **`Dynamite.Infrastructure`** | `AppDbContext` (EF Core 8), Entity Configurations, and Npgsql Repositories. |
| **`Dynamite.API`** | ASP.NET Core REST API, JWT Token Generator, SignalR Hub, and Rate Limiting. |
| **`Dynamite.Bot`** | Gateway Host, Event Handlers, SignalR Sync Client, and Background Schedulers. |
| **`Dynamite.Migrator`** | Autonomous EF Core Migration Console Application (runs in CI/CD or docker up). |
| **`Dynamite.Modules.*`** | Dedicated feature assemblies (Moderation, Security, Voice, Setup, etc.). |
| **`dynamite-dashboard`** | React 19 + TypeScript + Vite + TailwindCSS SPA with SignalR real-time hooks. |

---

## 🐳 Deployment (Docker Compose)

The entire microservice-like ecosystem is fully containerized and orchestrated via Docker Compose.

```bash
# 1. Configure environment variables
cp .env.example .env

# 2. Build and run all services (PostgreSQL, Migrator, API, Bot, Dashboard)
docker compose up --build -d

# 3. View real-time logs
docker logs -f dynamite_api
```

---

## 👨‍💻 Author & Contact

**Bá Lộc Vũ (DynamiteV)**
* **Focus Areas:** C# / .NET 8 Backend Engineering, Distributed Systems, Clean Architecture, Real-Time WebSockets (SignalR), and Cloud Deployment.
* **GitHub Profile:** [github.com/balocvu3105-dd](https://github.com/balocvu3105-dd)
