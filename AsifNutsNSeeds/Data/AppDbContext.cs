using AsifNutsNSeeds.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace AsifNutsNSeeds.Data
{
	public class AppDbContext:IdentityDbContext <ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product_Branch>().HasKey(pb => new
            {
                pb.Id,
                pb.ProductID

            });

            modelBuilder.Entity<Product_Branch>().HasOne(p => p.Product).WithMany(pb => pb.Product_Branches).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<Product_Branch>().HasOne(p => p.Branch).WithMany(pb => pb.Product_Branches).HasForeignKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Product_Branch> Product_Branches{ get; set; }
        public DbSet<ProductNotification> ProductNotifications { get; set; }

        //orders

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
