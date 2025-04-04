using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MilkTea.Core.ViewModels;
using MilkTea.Services.UserServices;
using System.Text.Json;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;

    public LoginModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public LoginViewModel LoginViewModel { get; set; } = new LoginViewModel();

    public string ErrorMessage { get; set; }

    public void OnGet()
    {
        var currentUserData = Request.Cookies["UserInfo"];

        if (currentUserData != null)
        {
            Response.Redirect("/Home");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.AuthenticateAsync(LoginViewModel);
            if (user != null)
            {
                if (user.Role == "Admin" || user.Role == "Staff")
                {
                    var userDataList = new List<string> { user.Username, user.FullName, user.Role };
                    var currentUserData = JsonSerializer.Serialize(userDataList);

                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(30),
                        HttpOnly = true
                    };

                    Response.Cookies.Append("UserInfo", currentUserData, cookieOptions);

                    return RedirectToPage("/Home");
                }
                else
                {
                    ErrorMessage = "You do not have permission to access this page.";
                }
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }

        return Page();
    }
}
