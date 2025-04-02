using System.ComponentModel.DataAnnotations;

namespace MilkTea.Core.ViewModels
{
	public class UserViewModel
	{
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
		[StringLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự")]
		public string Username { get; set; } = null!;

		[Required(ErrorMessage = "Mật khẩu không được để trống")]
		[StringLength(100, ErrorMessage = "Mật khẩu tối đa 100 ký tự")]
		public string Password { get; set; } = null!;

		[Required(ErrorMessage = "Họ và tên không được để trống")]
		[StringLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
		public string FullName { get; set; } = null!;

		[Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
		[StringLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Vai trò không được để trống")]
		[StringLength(20, ErrorMessage = "Vai trò tối đa 20 ký tự")]
		public string Role { get; set; } = null!;

		[StringLength(250, ErrorMessage = "Địa chỉ tối đa 250 ký tự")]
		public string? ShippingAddress { get; set; }

		public bool IsActive { get; set; }
	}
}
