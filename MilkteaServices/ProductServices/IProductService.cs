using MilkTea.Core.ViewModels;

namespace MilkTea.Services.ProductServices
{
	public interface IProductService
	{
		Task<IEnumerable<ProductViewModel>> GetPaginatedProductsAsync(int pageIndex, int pageSize);
		Task<ProductViewModel> GetProductByIdAsync(int productId);
		Task<string> AddProductAsync(ProductViewModel productViewModel);
		Task<string> UpdateProductAsync(int productId, ProductViewModel productViewModel);
		Task<string> DeleteProductAsync(int productId);
	}
}
