using AutoMapper;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaWeb.ViewModels;

namespace MilkTea.Repository.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RegisterViewModel, User>();
			CreateMap<ToppingViewModel, Topping>();
			CreateMap<CategoryViewModel, Category>();
			CreateMap<ProductViewModel, Product>();
		}
	}
}
