using Switchly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Switchly.Persistence.Db.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.Name).IsUnique();
        builder.Property(o => o.Name).IsRequired().HasMaxLength(200);
        builder.Property(o => o.CreatedAt).IsRequired();
    }
}