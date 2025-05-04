using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.Domain.Entities;

namespace Switchly.Persistence.Configurations;

public class FlagEnvironmentConfiguration : IEntityTypeConfiguration<FlagEnvironment>
{
    public void Configure(EntityTypeBuilder<FlagEnvironment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(x => x.Organization)
            .WithMany(o => o.FlagEnvironments)
            .HasForeignKey(x => x.OrganizationId);
    }
}