using Microsoft.EntityFrameworkCore;
using Switchly.Domain.Entities;
using Environment = System.Environment;

namespace Switchly.Persistence.Db;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();
    public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();
    public DbSet<FlagEnvironment> FlagEnvironments { get; set; } = null!;
    public DbSet<FeatureFlagEnvironment> FeatureFlagEnvironments { get; set; } = null!;


    public DbSet<SegmentRule> SegmentRules => Set<SegmentRule>();
    public DbSet<SegmentExpression> SegmentExpressions => Set<SegmentExpression>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fluent API ayarlarını otomatik yükle
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
