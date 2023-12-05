using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : DbContext {

    public DataContext() { }

    public DataContext(DbContextOptions options) : base (options) { }

    public DbSet<AppUser> AppUser { get; set; }

}

