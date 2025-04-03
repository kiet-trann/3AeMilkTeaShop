using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTea.Services.SignalR;
using MilkTea.Services.ToppingServices;

namespace MilkTeaAdminWeb.Pages.Toppings
{
    public class DeleteModel : PageModel
    {
        private readonly IToppingService _toppingService;
        private readonly IHubContext<SignalHub> _hubContext;

        public DeleteModel(IToppingService toppingService, IHubContext<SignalHub> hubContext)
        {
            _toppingService = toppingService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public ToppingViewModel ToppingViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                TempData["FailedMessage"] = "ID Không Được Trống!";
                return RedirectToPage("./Index");
            }

            var topping = await _toppingService.GetToppingByIdAsync(id.Value);

            if (topping == null)
            {
                TempData["FailedMessage"] = "Topping Không Có Sẵn!";
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var result = await _toppingService.DeleteToppingAsync((int)id);

            if (result == "Xóa topping thành công")
            {
                TempData["SuccessMessage"] = result;
                await _hubContext.Clients.All.SendAsync("LoadPage", "Toppings");
                return RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}
