using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Core.ViewModels;
using MilkTea.Services.SignalR;
using MilkTea.Services.ToppingServices;

namespace MilkTeaAdminWeb.Pages.Toppings
{
	public class EditModel : PageModel
	{
		private readonly IToppingService _toppingService;
        private readonly IHubContext<SignalHub> _hubContext;

        public EditModel(IToppingService toppingService, IHubContext<SignalHub> hubContext)
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
                TempData["FailedMessage"] = "ID Không Được Để Trống!";
                return RedirectToPage("./Index");
			}

			var topping = await _toppingService.GetToppingByIdAsync((int)id);
			if (topping == null)
			{
                TempData["FailedMessage"] = "Topping Không Có Sẵn!";
                return RedirectToPage("./Index");
			}

			ToppingViewModel = topping;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var result = await _toppingService.UpdateToppingAsync(ToppingViewModel);

			if (result != "Cập nhật topping thành công")
			{
                TempData["SuccessMessage"] = result;
                await _hubContext.Clients.All.SendAsync("LoadPage", "Toppings");
                return RedirectToPage("./Index");
			}
            TempData["FailedMessage"] = result;
            return RedirectToPage("./Index");
		}
	}
}
