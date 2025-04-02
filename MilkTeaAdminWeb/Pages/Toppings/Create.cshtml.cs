using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Core.ViewModels;
using MilkTea.Services.SignalR;
using MilkTea.Services.ToppingServices;

namespace MilkTeaAdminWeb.Pages.Toppings
{
    public class CreateModel : PageModel
    {
        private readonly IToppingService _toppingService;
        private readonly IHubContext<SignalHub> _hubContext;

        public CreateModel(IToppingService toppingService, IHubContext<SignalHub> hubContext)
        {
            _toppingService = toppingService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public ToppingViewModel ToppingRequestModel { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _toppingService.AddToppingAsync(ToppingRequestModel);

            if (result == "Thêm topping thành công")
            {
                TempData["SuccessMessage"] = result;
                await _hubContext.Clients.All.SendAsync("LoadPage", "Toppings");
                return Page();
            }

            TempData["FailedMessage"] = result;
            return RedirectToPage("./Index");
        }
    }
}
