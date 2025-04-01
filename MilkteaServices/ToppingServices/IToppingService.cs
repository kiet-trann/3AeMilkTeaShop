using MilkTea.Core.ViewModels;

namespace MilkTea.Services.ToppingServices
{
	public interface IToppingService
	{
		Task<IEnumerable<ToppingViewModel>> GetPaginatedToppingsAsync(int pageIndex, int pageSize);
		Task<ToppingViewModel> GetToppingByIdAsync(int toppingId);
		Task<string> AddToppingAsync(ToppingViewModel toppingViewModel);
		Task<string> UpdateToppingAsync(int toppingId, ToppingViewModel toppingViewModel);
		Task<string> DeleteToppingAsync(int toppingId);
	}
}
