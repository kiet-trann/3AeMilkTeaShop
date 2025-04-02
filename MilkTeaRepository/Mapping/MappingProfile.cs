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
			// User
			CreateMap<RegisterViewModel, User>();
			CreateMap<UserViewModel, User>();
			CreateMap<User, UserViewModel>();
            CreateMap<LoginViewModel, User>();

            // Topping
            CreateMap<ToppingViewModel, Topping>();

			// Category
			CreateMap<CategoryViewModel, Category>();
			CreateMap<Category, CategoryViewModel>();

			//Product
			CreateMap<ProductViewModel, Product>();
		}
	}
}
