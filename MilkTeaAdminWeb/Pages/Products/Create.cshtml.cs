using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ProductServices;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public ProductViewModel ProductRequestModel { get; set; } = default!;

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Categories = await _categoryService.GetAvailableCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _productService.AddProductAsync(ProductRequestModel);

            if (result != "Thêm sản phẩm thành công.")
            {
                ModelState.AddModelError(string.Empty, result);
                return RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}
