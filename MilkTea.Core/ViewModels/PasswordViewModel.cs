using System.ComponentModel.DataAnnotations;

namespace MilkTea.Core.ViewModels
{
	public class PasswordViewModel
	{
		public int UserId { get; set; }
		[Required(ErrorMessage = "Mật khẩu không được để trống")]
		[MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
		public string Password { get; set; } = null!;

		[Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
		[Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
		public string ConfirmPassword { get; set; } = null!;
	}
}
