using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : DbContext {

    public DataContext() { }

    public DataContext(DbContextOptions options) : base (options) { }

    public DbSet<AppUser> AppUser { get; set; }

    public DbSet<Authentication> Authentication { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Authentication>()
            .HasKey(a => a.AppUserId);

        modelBuilder.Entity<AppUser>()
            .HasOne(a => a.Authentication)
            .WithOne(b => b.AppUser)
            .HasForeignKey<Authentication>(b => b.AppUserId);
    }

}

