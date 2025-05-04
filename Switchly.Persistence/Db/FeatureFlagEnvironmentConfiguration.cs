using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.Domain.Entities;

public class FeatureFlagEnvironmentConfiguration : IEntityTypeConfiguration<FeatureFlagEnvironment>
{
    public void Configure(EntityTypeBuilder<FeatureFlagEnvironment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.FeatureFlag)
            .WithMany(f => f.FeatureFlagEnvironments)
            .HasForeignKey(x => x.FeatureFlagId);

        builder.HasOne(x => x.FlagEnvironment)
            .WithMany(e => e.FeatureFlagEnvironments)
            .HasForeignKey(x => x.FlagEnvironmentId);
    }
}