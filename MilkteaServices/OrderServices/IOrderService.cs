﻿using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;

namespace MilkTea.Services.OrderServices
{
	public interface IOrderService
	{
        Task<PaginatingResult<OrderViewModel>> GetPaginatedOrdersAsync(int pageNumber, int pageSize);
        Task<OrderViewModel> GetOrderByIdAsync(int orderId);
        Task<List<OrderDetailViewModel>> GetOrderDetailsForOrder(int orderId);
        Task<string> UpdateOrderStatusAsync(int orderId, string newStatus);

    }
}
