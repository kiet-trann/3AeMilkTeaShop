using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;

namespace MilkTea.Services.ProductServices
{
	public interface IProductService
	{
		Task<PaginatingResult<Product>> GetPaginatedProductsAsync(int pageIndex, int pageSize);
		Task<ProductViewModel> GetProductByIdAsync(int productId);
		Task<string> AddProductAsync(ProductViewModel productViewModel);
		Task<string> UpdateProductAsync(ProductViewModel productViewModel);
		Task<string> DeleteProductAsync(int productId);
	}
}
