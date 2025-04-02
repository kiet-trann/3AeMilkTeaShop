using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.Pagination;
using MilkTea.Repository.Model;
using MilkTea.Services.ProductServices;

namespace MilkTeaAdminWeb.Pages.Products
{
	public class IndexModel : PageModel
	{
		private readonly IProductService _productService;

		public IndexModel(IProductService productService)
		{
			_productService = productService;
		}

		public PaginatingResult<Product> PaginatedProducts { get; set; } = default!;

		public int PageSize { get; set; } = 5;

		public async Task OnGetAsync(int pageIndex = 1)
		{
			PaginatedProducts = await _productService.GetPaginatedProductsAsync(pageIndex, PageSize);
		}
	}
}
