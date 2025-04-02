using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public CategoryViewModel CategoryRequestModel { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _categoryService.AddCategoryAsync(CategoryRequestModel);

            if (result != "Thêm danh mục thành công")
            {
                ModelState.AddModelError(string.Empty, result);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
