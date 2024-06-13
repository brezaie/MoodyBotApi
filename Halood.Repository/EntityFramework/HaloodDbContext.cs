using Halood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Halood.Repository.EntityFramework;

public class HaloodDbContext : DbContext
{
    public HaloodDbContext(DbContextOptions<HaloodDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserSatisfaction> UserSatisfactions { get; set; }
    public DbSet<UserEmotion> UserEmotions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }

    public override int SaveChanges()
    {
        ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added).ToList()
            .ForEach(x => x.Property("CreatedDate").CurrentValue = DateTimeOffset.Now);
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added).ToList()
            .ForEach(x => x.Property("CreatedDate").CurrentValue = DateTimeOffset.Now);
        return await base.SaveChangesAsync(cancellationToken);
    }
}
