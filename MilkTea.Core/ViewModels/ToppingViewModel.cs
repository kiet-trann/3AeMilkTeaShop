using System.ComponentModel.DataAnnotations;

namespace MilkTea.Core.ViewModels
{
	public class ToppingViewModel
	{
		[Required(ErrorMessage = "Tên topping là bắt buộc.")]
		[StringLength(100, ErrorMessage = "Tên topping không được vượt quá 100 ký tự.")]
		public string ToppingName { get; set; } = null!;

		[Range(0, 1000000, ErrorMessage = "Giá topping phải lớn hơn hoặc bằng 0.")]
		public decimal Price { get; set; }

		public bool IsAvailable { get; set; }
	}
}
