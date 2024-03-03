using AsifNutsNSeeds.Data.Base;
using AsifNutsNSeeds.Data.ViewModels;
using AsifNutsNSeeds.Models;

namespace AsifNutsNSeeds.Data.Services
{
	public interface IProductsService : IEntityBaseRepository<Product>
	{
		Task<Product> GetProductByIdAsync(int id);
        Task<NewProductDropdownsVM> GetNewProductDropdownsValues();
        Task AddNewProductAsync(NewProductVM data);
        Task UpdateProductAsync(NewProductVM data);
    }
}
