using ShoppingCartAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // protected override void OnModelCreating(ModelBuilder builder)
        // {

        //     base.OnModelCreating(builder);

        //     builder.Entity<ProductCategory>()
        //    .HasIndex(u => u.Name)
        //    .IsUnique();

        //     builder.Entity<Product>()
        //         .Property(p => p.Price)
        //         .HasColumnType("decimal(18,2)");

        //     builder.Entity<Order>(entity =>
        //     {
        //         entity.HasKey(o => o.Id);
        //         entity.HasMany(o => o.OrderItems)
        //               .WithOne(oi => oi.Order)
        //               .HasForeignKey(oi => oi.OrderId);
        //     });

        //     builder.Entity<OrderItem>(entity =>
        //     {
        //         entity.HasKey(oi => oi.Id);
        //     });

        //     builder.Entity<Cart>(entity =>
        //     {
        //         entity.HasKey(c => c.Id);
        //         entity.HasMany(c => c.CartItems)
        //               .WithOne(ci => ci.Cart)
        //               .HasForeignKey(ci => ci.CartId);
        //     });

        //     builder.Entity<CartItem>(entity =>
        //     {
        //         entity.HasKey(ci => ci.Id);
        //     });

        // }
    }
}
