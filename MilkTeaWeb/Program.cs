using MilkTea.Repository.Mapping;
using MilkTea.Repository.Model;
using MilkTea.Services.ProductServices;
using MilkTea.Services.ToppingServices;
using MilkTea.Services.UserServices;
using MilkTeaRepository.UnitOfWork;
using MilkteaServices.CategoryServices;
using MilkTeaWeb.Components;

namespace MilkTeaWeb;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ThreeBrothersMilkTeaShopContext>();
		builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
		builder.Services.AddScoped<IUserService, UserService>();
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork> ();
		builder.Services.AddScoped<IToppingService, ToppingService>();
		builder.Services.AddScoped<IProductService, ProductService>();
		builder.Services.AddScoped<ICategoryService, CategoryService>();
		builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

		var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
