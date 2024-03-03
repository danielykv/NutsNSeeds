using AsifNutsNSeeds.Data.Base;
using AsifNutsNSeeds.Data.ViewModels;
using AsifNutsNSeeds.Models;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Data.Services
{
	public class ProductsService : EntityBaseRepository<Product>, IProductsService
	{
		private readonly AppDbContext _context;
		public ProductsService(AppDbContext context) : base(context)
		{
			_context = context;	
		}

        public async Task AddNewProductAsync(NewProductVM data)
        {
            var newProduct = new Product()
            {
                ProductName = data.ProductName,
                ProductDescription = data.ProductDescription,
                ProductPrice = data.ProductPrice,
                ImageURL = data.ImageURL,
                CountryID = data.CountryID,
                productCategory = data.ProductCategory,
                ProducerID = data.ProducerID
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            //Add Product branches
            foreach (var branchId in data.BranchIds)
            {
                var newBranchProduct = new Product_Branch()
                {
                    ProductID = newProduct.Id,
                    Id = branchId
                };
                await _context.Product_Branches.AddAsync(newBranchProduct);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<NewProductDropdownsVM> GetNewProductDropdownsValues()
        {
            var response = new NewProductDropdownsVM()
            {

                Branches = await _context.Branches.OrderBy(n => n.BranchName).ToListAsync(),
                Countries = await _context.Countries.OrderBy(n => n.CountryName).ToListAsync(),
                Producers = await _context.Producers.OrderBy(n => n.ProducerName).ToListAsync()

            };


            return response;
        }

        public async Task<Product> GetProductByIdAsync(int id)
		{
			
			var productDetails = await _context.Products.Include(c => c.Country).Include(p => p.Producer).Include(pb => pb.Product_Branches).ThenInclude(b => b.Branch).FirstOrDefaultAsync(n => n.Id == id);
			return productDetails;
		}

        public async Task UpdateProductAsync(NewProductVM data)
        {
            var dbProduct = await _context.Products.FirstOrDefaultAsync(n => n.Id == data.Id);

            if (dbProduct != null)
            {
                dbProduct.ProductName = data.ProductName;
                dbProduct.ProductDescription = data.ProductDescription;
                dbProduct.ProductPrice = data.ProductPrice;
                dbProduct.ImageURL = data.ImageURL;
                dbProduct.CountryID = data.CountryID;
                dbProduct.productCategory = data.ProductCategory;
                dbProduct.ProducerID= data.ProducerID;
                await _context.SaveChangesAsync();
            }

            //Remove existing actors
            var existingBranchesDb = _context.Product_Branches.Where(n => n.ProductID == data.Id).ToList();
            _context.Product_Branches.RemoveRange(existingBranchesDb);
            await _context.SaveChangesAsync();

            //Add Product branches
            foreach (var branchId in data.BranchIds)
            {
                var newActorMovie = new Product_Branch()
                {
                    ProductID = data.Id,
                    Id = branchId
                };
                await _context.Product_Branches.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync();
        }
    }
}
