using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;

namespace MilkteaServices.CategoryServices
{
	public interface ICategoryService
	{
		Task<string> AddCategoryAsync(CategoryViewModel categoryViewModel);
		Task<string> UpdateCategoryAsync(string categoryId, CategoryViewModel categoryViewModel);
		Task<string> DeleteCategoryAsync(int categoryId);
		Task<IEnumerable<Category>> GetPaginatedCategoriesAsync(int pageIndex, int pageSize);
	}
}
