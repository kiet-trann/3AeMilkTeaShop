using MilkTea.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public DetailsModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public CategoryViewModel Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            // Gọi service để lấy thông tin danh mục
            var categoryViewModel = await _categoryService.GetCategoryByIdAsync(id.Value);

            Category = categoryViewModel;
            return Page();
        }
    }
}
