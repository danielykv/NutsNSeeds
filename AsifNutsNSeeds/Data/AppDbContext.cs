using AsifNutsNSeeds.Models;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product_Branch>().HasKey(pb => new
            {
                pb.BranchID,
                pb.ProductID

            });

            modelBuilder.Entity<Product_Branch>().HasOne(p => p.Product).WithMany(pb => pb.Product_Branches).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<Product_Branch>().HasOne(p => p.Branch).WithMany(pb => pb.Product_Branches).HasForeignKey(p => p.BranchID);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Branch> Product_Branches{ get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Producer> Producers { get; set; }
    }
}
