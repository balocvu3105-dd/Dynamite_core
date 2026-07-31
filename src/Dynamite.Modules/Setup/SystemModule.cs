// src/Dynamite.Modules/Setup/SystemModule.cs
namespace Dynamite.Modules.Setup;

using Discord;
using Discord.Interactions;
using Dynamite.Application.Interfaces;
using System.Threading.Tasks;

[Group("system", "System maintenance commands for bot admins")]
[DefaultMemberPermissions(GuildPermission.Administrator)]
public class SystemModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IBackupService _backupService;

    public SystemModule(IBackupService backupService)
    {
        _backupService = backupService;
    }

    [SlashCommand("backup", "Create a backup of the current server configurations to disk.")]
    public async Task BackupAsync()
    {
        await DeferAsync(ephemeral: true);
        var path = await _backupService.CreateBackupAsync();
        
        var embed = new EmbedBuilder()
            .WithTitle("💾 Backup Successful")
            .WithDescription("All server configurations (RolePanels, Ticket, Voice, etc.) have been safely backed up.")
            .AddField("Backup File", $"`{path}`")
            .WithColor(Color.Green)
            .Build();

        await FollowupAsync(embed: embed, ephemeral: true);
    }

    [SlashCommand("restore", "Restore the server configurations from the latest backup.")]
    public async Task RestoreAsync()
    {
        await DeferAsync(ephemeral: true);
        var (success, message) = await _backupService.RestoreBackupAsync();

        var embed = new EmbedBuilder()
            .WithTitle(success ? "🔄 Restore Successful" : "❌ Restore Failed")
            .WithDescription(message)
            .WithColor(success ? Color.Green : Color.Red)
            .Build();

        await FollowupAsync(embed: embed, ephemeral: true);
    }
}
