using Switchly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Switchly.Persistence.Db.Configurations;

public class FeatureFlagConfiguration : IEntityTypeConfiguration<FeatureFlag>
{
    public void Configure(EntityTypeBuilder<FeatureFlag> builder)
    {
        builder.HasKey(f => f.Id);
        builder.HasIndex(f => new { f.OrganizationId, f.Key }).IsUnique();
        builder.Property(f => f.Key).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Description).HasMaxLength(500);
        builder.Property(f => f.IsArchived).HasDefaultValue(false);
        builder.Property(f => f.CreatedAt).IsRequired();
        builder.Property(f => f.UpdatedAt).IsRequired();

        builder.HasOne(f => f.Organization)
            .WithMany(o => o.FeatureFlags)
            .HasForeignKey(f => f.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.FeatureFlagEnvironments)
            .WithOne(e => e.FeatureFlag)
            .HasForeignKey(e => e.FeatureFlagId);

        builder.HasMany(f => f.SegmentRules)
            .WithOne(r => r.FeatureFlag)
            .HasForeignKey(r => r.FeatureFlagId);
    }
}