using Dynamite.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dynamite.Infrastructure.Configurations;

public class ModuleFaultConfiguration : IEntityTypeConfiguration<ModuleFault>
{
    public void Configure(EntityTypeBuilder<ModuleFault> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ModuleName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Reason)
            .HasMaxLength(1000);

        builder.HasOne(x => x.GuildConfig)
            .WithMany(g => g.ModuleFaults)
            .HasForeignKey(x => x.GuildConfigId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
