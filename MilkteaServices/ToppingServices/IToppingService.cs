﻿using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;

namespace MilkTea.Services.ToppingServices
{
	public interface IToppingService
	{
        Task<PaginatingResult<ToppingViewModel>> GetPaginatedToppingsAsync(int pageIndex, int pageSize);
		List<ToppingViewModel> GetAllToppings();
		Task<ToppingViewModel> GetToppingByIdAsync(int toppingId);
		Task<string> AddToppingAsync(ToppingViewModel toppingViewModel);
		Task<string> UpdateToppingAsync(ToppingViewModel toppingViewModel);
		Task<string> DeleteToppingAsync(int toppingId);
	}
}
