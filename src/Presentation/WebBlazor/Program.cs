using HomeInv.Persistence;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using WebBlazor.Areas.Identity;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Business.Services;
using HomeInv.Business.Handlers;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HomeInventoryDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<HomeInv.Persistence.Dbo.User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<HomeInventoryDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<HomeInv.Persistence.Dbo.User>>();

builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IHomeUserService, HomeUserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISizeUnitService, SizeUnitService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserSettingService, UserSettingService>();
builder.Services.AddScoped<IItemDefinitionService, ItemDefinitionService>();
builder.Services.AddScoped<IItemStockService, ItemStockService>();
builder.Services.AddScoped<IUpdateItemStockHandler, UpdateItemStockHandler>();
builder.Services.AddScoped<IUpdateRemainingStockAmountHandler, UpdateRemainingStockAmountHandler>();
builder.Services.AddScoped<ISmartUpdateItemStockHandler, SmartUpdateItemStockHandler>();

builder.Services.AddMudServices();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    HomeInventoryDbContext context = scope.ServiceProvider.GetRequiredService<HomeInventoryDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
