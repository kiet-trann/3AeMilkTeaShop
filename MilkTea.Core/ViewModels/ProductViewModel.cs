using System.ComponentModel.DataAnnotations;

namespace MilkTea.Core.ViewModels
{
	public class ProductViewModel
	{
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
		[StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
		public string ProductName { get; set; }
        public string? Description { get; set; } = null!;

        [Required(ErrorMessage = "Danh mục sản phẩm là bắt buộc.")]
		public int CategoryId { get; set; }

		[Required(ErrorMessage = "Giá size S là bắt buộc.")]
		[Range(0, double.MaxValue, ErrorMessage = "Giá size S phải là một số dương.")]
		public decimal PriceS { get; set; }

		[Required(ErrorMessage = "Giá size M là bắt buộc.")]
		[Range(0, double.MaxValue, ErrorMessage = "Giá size M phải là một số dương.")]
		public decimal PriceM { get; set; }

		[Required(ErrorMessage = "Giá size L là bắt buộc.")]
		[Range(0, double.MaxValue, ErrorMessage = "Giá size L phải là một số dương.")]
		public decimal PriceL { get; set; }

		[Required(ErrorMessage = "Trạng thái khả dụng của size S là bắt buộc.")]
		public bool IsAvailableS { get; set; }

		[Required(ErrorMessage = "Trạng thái khả dụng của size M là bắt buộc.")]
		public bool IsAvailableM { get; set; }

		[Required(ErrorMessage = "Trạng thái khả dụng của size L là bắt buộc.")]
		public bool IsAvailableL { get; set; }
		public string? ImageUrl { get; set; }
        public CategoryViewModel Category { get; set; } = null!;
    }
}
