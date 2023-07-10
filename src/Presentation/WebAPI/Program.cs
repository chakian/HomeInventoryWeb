using HomeInv.Business.Handlers;
using HomeInv.Business.Services;
using HomeInv.Common.Configuration;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<HomeInventoryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HomeInvConnectionString")));

builder.Services.AddAuthentication();
builder.Services.AddIdentity<HomeInv.Persistence.Dbo.User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<HomeInventoryDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

/// Application service injections
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IHomeUserService, HomeUserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<ISizeUnitService, SizeUnitService>(); // TODO: Throws an error about a MemoryCache dependency
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserSettingService, UserSettingService>();
builder.Services.AddScoped<IItemDefinitionService, ItemDefinitionService>();
builder.Services.AddScoped<IItemStockService, ItemStockService>();
builder.Services.AddScoped<IUpdateItemStockHandler, UpdateItemStockHandler>();
builder.Services.AddScoped<IUpdateRemainingStockAmountHandler, UpdateRemainingStockAmountHandler>();
builder.Services.AddScoped<ISmartUpdateItemStockHandler, SmartUpdateItemStockHandler>();

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.Configure<EmailSenderOptions>(builder.Configuration.GetSection("EmailSenderOptions"));
/// Application service injections

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    HomeInventoryDbContext context = scope.ServiceProvider.GetRequiredService<HomeInventoryDbContext>();
    context.Database.Migrate();
}

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
