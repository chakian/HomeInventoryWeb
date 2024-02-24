using HomeInv.Business.Handlers;
using HomeInv.Business.Services;
using HomeInv.Common.Configuration;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace HomeInv.WebApi;

public static class DependencyModule
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<HomeInventoryDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IHomeUserService, HomeUserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISizeUnitService, SizeUnitService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserSettingService, UserSettingService>();
        services.AddScoped<IItemDefinitionService, ItemDefinitionService>();
        services.AddScoped<IItemStockService, ItemStockService>();

        return services.AddServiceHandlers();
    }

    private static IServiceCollection AddServiceHandlers(this IServiceCollection services)
    {
        services.AddScoped<IUpdateItemStockHandler, UpdateItemStockHandler>();
        services.AddScoped<IUpdateRemainingStockAmountHandler, UpdateRemainingStockAmountHandler>();
        services.AddScoped<ISmartUpdateItemStockHandler, SmartUpdateItemStockHandler>();

        return services;
    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<EmailSenderOptions>(configuration.GetSection("EmailSenderOptions"));
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddTransient<IEmailSender, EmailSenderService>();

        return services;
    }
}