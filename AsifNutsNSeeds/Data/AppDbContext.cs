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
            modelBuilder.Entity<Product_Branch>().HasKey(am => new
            {
                am.BranchID,
                am.ProductID

            });

            modelBuilder.Entity<Product_Branch>().HasOne(m => m.Product).WithMany(am => am.Product_Branches).HasForeignKey(m => m.ProductID);
            modelBuilder.Entity<Product_Branch>().HasOne(m => m.Branch).WithMany(am => am.Product_Branches).HasForeignKey(m => m.BranchID);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Branch> Product_Branches{ get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Producer> Producers { get; set; }
    }
}
