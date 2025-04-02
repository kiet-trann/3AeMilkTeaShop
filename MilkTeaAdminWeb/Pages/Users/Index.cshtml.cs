using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Services.UserServices;
using MilkTea.Repository.Model;
using MilkTea.Core.Pagination;

namespace MilkTeaAdminWeb.Pages.Users
{
	public class IndexModel : PageModel
	{
		private readonly IUserService _userService;

		public IndexModel(IUserService userService)
		{
			_userService = userService;
		}

		public PaginatingResult<User> PaginatedUsers { get; set; } = default!;

        public int PageSize { get; set; } = 5;

        public async Task OnGetAsync(int pageIndex = 1)
		{
            PaginatedUsers = await _userService.GetPaginatedUsersAsync(pageIndex, PageSize);
		}
	}
}
