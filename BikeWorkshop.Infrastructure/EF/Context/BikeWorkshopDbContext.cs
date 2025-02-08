using BikeWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.Infrastructure.EF.Context;

public class BikeWorkshopDbContext : DbContext
{
    public DbSet<ClientData> ClientData { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceToOrder> ServiceToOrders { get; set; }
    public DbSet<Summary> Summaries { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public BikeWorkshopDbContext(DbContextOptions<BikeWorkshopDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(BikeWorkshopDbContext).Assembly);
}
