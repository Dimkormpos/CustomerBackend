using CustomerBackend.Database;
using Microsoft.EntityFrameworkCore;

public class CustomerBackendContext: DbContext
{
    public CustomerBackendContext(DbContextOptions<CustomerBackendContext> options)
        : base(options)
    {
    }
    public DbSet<Products> Products { get; set; }
    public DbSet<Customers> Customers { get; set; }
    public DbSet<Purchases> Purchases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the one-to-many relationship
        modelBuilder.Entity<Products>()
            .HasMany(p => p.Purchases)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        modelBuilder.Entity<Customers>()
           .HasMany(p => p.Purchases)
           .WithOne(p => p.Customer)
           .HasForeignKey(p => p.CustomerId);
    }
}
