// src/Dynamite.Infrastructure/Services/BackupService.cs
namespace Dynamite.Infrastructure.Services;

using System.Text.Json;
using Dynamite.Application.Interfaces;
using Dynamite.Core.Entities;
using Dynamite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class BackupData
{
    public List<GuildConfig> GuildConfigs { get; set; } = new();
    public List<RolePanel> RolePanels { get; set; } = new();
    public List<RolePanelItem> RolePanelItems { get; set; } = new();
    public List<TempVoiceConfig> TempVoiceConfigs { get; set; } = new();
    public List<TicketConfig> TicketConfigs { get; set; } = new();
}

public class BackupService : IBackupService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BackupService> _logger;
    private readonly string _backupPath;

    public BackupService(IServiceScopeFactory scopeFactory, ILogger<BackupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        
        var logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        Directory.CreateDirectory(logsDir);
        _backupPath = Path.Combine(logsDir, "database_backup.json");
    }

    public async Task<string> CreateBackupAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var data = new BackupData
        {
            GuildConfigs = await db.GuildConfigs.AsNoTracking().ToListAsync(),
            RolePanels = await db.RolePanels.AsNoTracking().ToListAsync(),
            RolePanelItems = await db.RolePanelItems.AsNoTracking().ToListAsync(),
            TempVoiceConfigs = await db.TempVoiceConfigs.AsNoTracking().ToListAsync(),
            TicketConfigs = await db.TicketConfigs.AsNoTracking().ToListAsync()
        };

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions 
        { 
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        });

        await File.WriteAllTextAsync(_backupPath, json);
        _logger.LogInformation("Database backup created successfully at {Path}", _backupPath);
        
        return _backupPath;
    }

    public async Task<(bool Success, string Message)> RestoreBackupAsync()
    {
        if (!File.Exists(_backupPath))
            return (false, "No backup file found.");

        try
        {
            var json = await File.ReadAllTextAsync(_backupPath);
            var data = JsonSerializer.Deserialize<BackupData>(json, new JsonSerializerOptions 
            { 
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });

            if (data == null) return (false, "Invalid backup data.");

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.RolePanelItems.RemoveRange(db.RolePanelItems);
            db.RolePanels.RemoveRange(db.RolePanels);
            db.TempVoiceConfigs.RemoveRange(db.TempVoiceConfigs);
            db.TicketConfigs.RemoveRange(db.TicketConfigs);
            db.GuildConfigs.RemoveRange(db.GuildConfigs);
            
            await db.SaveChangesAsync();

            await db.GuildConfigs.AddRangeAsync(data.GuildConfigs);
            await db.SaveChangesAsync();

            await db.RolePanels.AddRangeAsync(data.RolePanels);
            await db.RolePanelItems.AddRangeAsync(data.RolePanelItems);
            await db.TempVoiceConfigs.AddRangeAsync(data.TempVoiceConfigs);
            await db.TicketConfigs.AddRangeAsync(data.TicketConfigs);
            
            await db.SaveChangesAsync();

            _logger.LogInformation("Database restored successfully from {Path}", _backupPath);
            return (true, "Restore completed successfully. Old setups and buttons should now be linked back.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore backup");
            return (false, $"Restore failed: {ex.Message}");
        }
    }
}
