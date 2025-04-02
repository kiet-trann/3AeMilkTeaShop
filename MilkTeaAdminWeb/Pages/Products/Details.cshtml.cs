using MilkTea.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Services.ProductServices;

namespace MilkTeaAdminWeb.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

        public ProductViewModel Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            // Gọi ProductService để lấy thông tin sản phẩm
            var productViewModel = await _productService.GetProductByIdAsync((int)id);

            if (productViewModel == null)
            {
                return RedirectToPage("./Index");
            }

            Product = productViewModel;
            return Page();
        }
    }
}
