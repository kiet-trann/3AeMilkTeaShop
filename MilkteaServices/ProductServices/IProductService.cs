using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;

namespace MilkTea.Services.ProductServices
{
	public interface IProductService
	{
		Task<PaginatingResult<ProductViewModel>> GetPaginatedProductsAsync(int pageIndex, int pageSize);
		Task<PaginatingResult<ProductViewModel>> GetProductByCategory(int categoryId, int pageNumber, int pageSize);
		Task<ProductViewModel> GetProductByIdAsync(int productId);
		Task<string> AddProductAsync(ProductViewModel productViewModel);
		Task<string> UpdateProductAsync(ProductViewModel productViewModel);
		Task<string> DeleteProductAsync(int productId);
	}
}
