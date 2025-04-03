using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ProductServices;
using MilkTea.Services.SignalR;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHubContext<SignalHub> _hubContext;

        public EditModel(IProductService productService, ICategoryService categoryService, IHubContext<SignalHub> hubContext)
        {
            _productService = productService;
            _categoryService = categoryService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public ProductViewModel ProductViewModel { get; set; } = default!;
        public IEnumerable<CategoryViewModel> Categories { get; set; }

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

            Categories = _categoryService.GetAvailableCategories();
            ProductViewModel = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
			var result = await _productService.UpdateProductAsync(ProductViewModel);

            if (result == "Cập nhật sản phẩm thành công!")
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
