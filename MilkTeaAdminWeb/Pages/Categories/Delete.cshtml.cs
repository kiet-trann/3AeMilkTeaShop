using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkteaServices.CategoryServices;
using MilkTea.Repository.Model;

namespace MilkTeaAdminWeb.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public DeleteModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin danh mục
            var category = await _categoryService.GetCategoryByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = new Category
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    IsActive = category.IsActive
                };
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var result = await _categoryService.DeleteCategoryAsync(id.Value);

            if (result == "Xóa danh mục thành công")
            {
                ViewData["Message"] = result;
            }
            else
            {
                ViewData["Message"] = result;
            }

            return RedirectToPage("./Index");
        }
    }
}
