using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ProductServices;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public ProductViewModel ProductViewModel { get; set; } = default!;
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var product = await _productService.GetProductByIdAsync((int)id);
            if (product == null)
            {
                return RedirectToPage("./Index");
            }

            Categories = await _categoryService.GetAvailableCategoriesAsync();
            ProductViewModel = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
			var result = await _productService.UpdateProductAsync(ProductViewModel);

            if (result != "Cập nhật sản phẩm thành công")
            {
                ModelState.AddModelError(string.Empty, result);
                return RedirectToPage("./Index");
			}

            return RedirectToPage("./Index");
        }
    }
}
