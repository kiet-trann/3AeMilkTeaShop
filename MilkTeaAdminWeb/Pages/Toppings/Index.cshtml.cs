using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ToppingServices;

namespace MilkTeaAdminWeb.Pages.Toppings
{
    public class IndexModel : PageModel
    {
        private readonly IToppingService _toppingService;

        public PaginatingResult<ToppingViewModel> PaginatedToppings { get; set; } = default!;

        public int PageSize { get; set; } = 5;

        public IndexModel(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PaginatedToppings = await _toppingService.GetPaginatedToppingsAsync(pageIndex, PageSize);
        }
    }
}
