using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : DbContext {

    public DataContext() { }

    public DataContext(DbContextOptions options) : base(options) { }

    public DbSet<AppUser> AppUser { get; set; }

    public DbSet<Authentication> Authentication { get; set; }

    public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }

    public DbSet<Blog> Blog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Blog>()
            .HasKey(b => b.BlogId);

        modelBuilder.Entity<Blog>()
            .HasOne(b => b.AppUser)
            .WithMany(u => u.Blogs)
            .HasForeignKey(b => b.AppUserId);

        modelBuilder.Entity<Authentication>()
            .HasKey(a => a.AppUserId);

        modelBuilder.Entity<AppUser>()
            .HasOne(a => a.Authentication)
            .WithOne(b => b.AppUser)
            .HasForeignKey<Authentication>(b => b.AppUserId);

        modelBuilder.Entity<PasswordResetRequest>()
            .HasKey(p => p.PasswordRequestId);

        modelBuilder.Entity<AppUser>()
        .HasMany(a => a.PasswordResetRequests)
        .WithOne(p => p.AppUser)
        .HasForeignKey(p => p.AppUserId);
    }

}

