using Switchly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Switchly.Persistence.Db.Configurations;

public class SegmentRuleConfiguration : IEntityTypeConfiguration<SegmentRule>
{
    public void Configure(EntityTypeBuilder<SegmentRule> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Property).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Operator).IsRequired().HasMaxLength(50);
        builder.Property(r => r.Value).IsRequired().HasMaxLength(200);
        builder.Property(r => r.RolloutPercentage).IsRequired();

        builder.HasOne(r => r.FeatureFlag)
            .WithMany(f => f.SegmentRules)
            .HasForeignKey(r => r.FeatureFlagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
