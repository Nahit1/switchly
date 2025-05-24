using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.Domain.Entities;

namespace Switchly.Persistence.Db;

public class SegmentExpressionConfiguration:IEntityTypeConfiguration<SegmentExpression>
{
  public void Configure(EntityTypeBuilder<SegmentExpression> builder)
  {
    builder.ToTable("SegmentExpressions");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Operator)
      .HasMaxLength(20);

    // SegmentRule ilişkisi (optional)
    builder.HasOne(x => x.SegmentRule)
      .WithMany()
      .HasForeignKey(x => x.SegmentRuleId)
      .OnDelete(DeleteBehavior.Restrict);

    // Parent → Children self reference
    builder.HasOne(x => x.ParentExpression)
      .WithMany(x => x.Children)
      .HasForeignKey(x => x.ParentExpressionId)
      .OnDelete(DeleteBehavior.Restrict); // recursive silmeyi engelle

    // FeatureFlag ilişkisi
    builder.HasOne(x => x.FeatureFlag)
      .WithMany(x => x.SegmentExpressions)
      .HasForeignKey(x => x.FeatureFlagId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
