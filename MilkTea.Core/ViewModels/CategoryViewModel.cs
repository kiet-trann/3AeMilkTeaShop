using System.ComponentModel.DataAnnotations;

namespace MilkTea.Core.ViewModels
{
	public class CategoryViewModel
	{
		[Required(ErrorMessage = "Tên danh mục không được để trống")]
		[StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
		public string CategoryName { get; set; } = null!;

		[StringLength(250, ErrorMessage = "Mô tả không được vượt quá 250 ký tự")]
		public string? Description { get; set; }

		public bool IsActive { get; set; }
	}
}
