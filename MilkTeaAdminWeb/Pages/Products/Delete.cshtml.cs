using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ProductServices;
using MilkTea.Services.SignalR;

namespace MilkTeaAdminWeb.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IHubContext<SignalHub> _hubContext;

        public DeleteModel(IProductService productService, IHubContext<SignalHub> hubContext)
        {
            _productService = productService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public ProductViewModel Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                TempData["FailedMessage"] = "ID Không Được Trống!";
                return RedirectToPage("./Index");
            }

            var product = await _productService.GetProductByIdAsync((int)id);

            if (product == null)
            {
                TempData["FailedMessage"] = "Sản Phẩm Không Tồn Tại!";
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
            var result = await _productService.DeleteProductAsync((int)id);

            if (result == "Xóa sản phẩm thành công.")
            {
                TempData["SuccessMessage"] = result;
                await _hubContext.Clients.All.SendAsync("LoadPage", "Products");
                return RedirectToPage("./Index");
            }

            TempData["FailedMessage"] = result;
            return RedirectToPage("./Index");
        }
    }
}
