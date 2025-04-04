using System.ComponentModel.DataAnnotations;

namespace MilkTeaWeb.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Username không được để trống")]
		[MinLength(3, ErrorMessage = "Username phải có ít nhất 3 ký tự")]
		[MaxLength(50, ErrorMessage = "Username không được quá 50 ký tự")]
		public string Username { get; set; } = null!;

		[Required(ErrorMessage = "Mật khẩu không được để trống")]
		[MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
		public string Password { get; set; } = null!;

		[Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
		[Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
		public string ConfirmPassword { get; set; } = null!;

		[Required(ErrorMessage = "Họ và tên không được để trống")]
		[MaxLength(100, ErrorMessage = "Họ và tên không được quá 100 ký tự")]
		public string FullName { get; set; } = null!;

		[Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
		public string? PhoneNumber { get; set; }

		[MaxLength(100, ErrorMessage = "Địa chỉ không được quá 100 ký tự")]
		public string? ShippingAddress { get; set; }
	}
}
