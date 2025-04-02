using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;

namespace MilkteaServices.CategoryServices
{
    public interface ICategoryService
	{
		Task<string> AddCategoryAsync(CategoryViewModel categoryViewModel);
		Task<string> UpdateCategoryAsync(CategoryViewModel categoryViewModel);
		Task<string> DeleteCategoryAsync(int categoryId);
		Task<PaginatingResult<Category>> GetPaginatedCategoriesAsync(int pageIndex, int pageSize);
        Task<CategoryViewModel?> GetCategoryByIdAsync(int categoryId);
        Task<IEnumerable<CategoryViewModel>> GetAvailableCategoriesAsync();
    }
}
