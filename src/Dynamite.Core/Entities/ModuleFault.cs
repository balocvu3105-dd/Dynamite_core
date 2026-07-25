namespace Dynamite.Core.Entities;

public class ModuleFault : BaseEntity
{
    public Guid GuildConfigId { get; set; }
    
    /// <summary>
    /// Tên của module bị lỗi (vd: "Welcome", "Logging", "TempVoice")
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;
    
    /// <summary>
    /// Chi tiết thông báo lỗi (vd: "Missing Permissions", "Unknown Channel")
    /// </summary>
    public string Reason { get; set; } = string.Empty;
    
    /// <summary>
    /// Thời điểm bắt đầu báo lỗi
    /// </summary>
    public DateTime FaultedAt { get; set; } = DateTime.UtcNow;

    public GuildConfig GuildConfig { get; set; } = null!;
}
