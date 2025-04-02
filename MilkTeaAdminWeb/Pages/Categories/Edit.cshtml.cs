using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public CategoryViewModel CategoryViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var category = await _categoryService.GetCategoryByIdAsync((int)id);
            if (category == null)
            {
                return RedirectToPage("./Index");
            }

            CategoryViewModel = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _categoryService.UpdateCategoryAsync(CategoryViewModel);

            if (result != "Cập nhật danh mục thành công")
            {
                ModelState.AddModelError(string.Empty, result);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
