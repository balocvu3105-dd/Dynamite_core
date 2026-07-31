// src/Dynamite.Infrastructure/DependencyInjection.cs
namespace Dynamite.Infrastructure;

using Dynamite.Application.Interfaces;
using Dynamite.Core.Interfaces.Repositories;
using Dynamite.Infrastructure.Persistence;
using Dynamite.Infrastructure.Repositories;
using Dynamite.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IGuildConfigRepository, GuildConfigRepository>();
        services.AddScoped<IWarningRepository, WarningRepository>();
        services.AddScoped<IModerationRepository, ModerationRepository>();
        services.AddScoped<IBlacklistRepository, BlacklistRepository>();
        services.AddScoped<IAntiSpamRepository, AntiSpamRepository>();
        services.AddScoped<IAutoRoleRepository, AutoRoleRepository>();
        services.AddScoped<IRolePanelRepository, RolePanelRepository>();
        services.AddScoped<IGiveawayRepository, GiveawayRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IGuildPresenceRepository, GuildPresenceRepository>();
        services.AddScoped<ITempVoiceRepository, TempVoiceRepository>();
        services.AddScoped<IServerActivityLogRepository, ServerActivityLogRepository>();

        services.AddSingleton<IBackupService, BackupService>();

        return services;
    }
}