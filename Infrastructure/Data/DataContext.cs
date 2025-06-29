using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Table> Tables { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table>()
        .HasMany(t => t.Reservations)
        .WithOne(r => r.Table)
        .HasForeignKey(r => r.TableId);
    
        modelBuilder.Entity<Customer>()
        .HasMany(c => c.Reservations)
        .WithOne(r => r.Customer)
        .HasForeignKey(r => r.CustomerId);
    }

}
    