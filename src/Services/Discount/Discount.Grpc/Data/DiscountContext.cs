using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DbSet<Coupon> Coupones { get; set; } = default!;

    public DiscountContext(DbContextOptions<DiscountContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Coupon>()
            .HasData(
                new Coupon()
                {
                    Id = 1,
                    ProductName = "Iphone",
                    ProductDescription = "New Iphone",
                    Amount = 100,
                },
                new Coupon()
                {
                    Id = 2,
                    ProductName = "Samsung",
                    ProductDescription = "New Samsung",
                    Amount = 150,
                }
            );
    }
}
