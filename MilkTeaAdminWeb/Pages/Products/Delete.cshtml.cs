using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ProductServices;

namespace MilkTeaAdminWeb.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductViewModel Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            // Get product information
            var product = await _productService.GetProductByIdAsync(id.Value);

            if (product == null)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                Product = product;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var result = await _productService.DeleteProductAsync(id.Value);

            // Pass message back to the view
            if (result == "Xóa sản phẩm thành công.")
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
