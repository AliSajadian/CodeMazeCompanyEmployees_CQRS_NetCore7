using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Entities.Models;

namespace Repository;

public class RepositoryContext : IdentityDbContext<User>
// public class RepositoryContext : DbContext 
{ 
    public RepositoryContext(DbContextOptions options) : base(options) 
    { 

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    { 
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CompanyConfiguration()); 
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration()); 
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }

    public DbSet<Company>? Companies { get; set; } 
    public DbSet<Employee>? Employees { get; set; } 
}