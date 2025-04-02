using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ProductServices;
using MilkTea.Services.SignalR;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHubContext<SignalHub> _hubContext;

        public CreateModel(IProductService productService, ICategoryService categoryService, IHubContext<SignalHub> hubContext)
        {
            _productService = productService;
            _categoryService = categoryService;
            _hubContext = hubContext;
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
                TempData["SuccessMessage"] = result;
                await _hubContext.Clients.All.SendAsync("LoadPage", "Products");
                return RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}
