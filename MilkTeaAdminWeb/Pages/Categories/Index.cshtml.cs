using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Categories
{
	public class IndexModel : PageModel
	{
		private readonly ICategoryService _categoryService;

		public IndexModel(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

	
		public PaginatingResult<Category> PaginatedCategories { get; set; } = default!;
		 
		public int PageSize { get; set; } = 5;

		public async Task OnGetAsync(int pageIndex = 1)
		{
			PaginatedCategories = await _categoryService.GetPaginatedCategoriesAsync(pageIndex, PageSize); ;
		}
	}
}
