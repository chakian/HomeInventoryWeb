using HomeInv.Business.Handlers;
using HomeInv.Business.Services;
using HomeInv.Common.Configuration;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HomeInventoryDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<HomeInv.Persistence.Dbo.User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<HomeInventoryDbContext>();

            services.AddSession();
            //services.AddMemoryCache();

            services.AddRazorPages();

            //TODO: Changing culture: https://www.mikesdotnetting.com/article/345/localisation-in-asp-net-core-razor-pages-cultures
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                 {
                    new CultureInfo("tr-TR"),
                    //new CultureInfo("en"),
                    //new CultureInfo("de"),
                    //new CultureInfo("fr"),
                    //new CultureInfo("es"),
                    //new CultureInfo("ru"),
                    //new CultureInfo("ja"),
                    //new CultureInfo("ar"),
                    //new CultureInfo("zh"),
                    //new CultureInfo("en-GB")
                };
                options.DefaultRequestCulture = new RequestCulture("tr-TR");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.Configure<EmailSenderOptions>(Configuration);
            services.AddTransient<IEmailSenderService, EmailSenderService>();

            ConfigureInternalServices(services);
        }

        private void ConfigureInternalServices(IServiceCollection services)
        {
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IHomeUserService, HomeUserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISizeUnitService, SizeUnitService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IAreaUserService, AreaUserService>();
            services.AddScoped<IUserSettingService, UserSettingService>();
            services.AddScoped<IItemDefinitionService, ItemDefinitionService>();
            services.AddScoped<IItemStockService, ItemStockService>();

            services.AddScoped<IUpdateItemStockHandler, UpdateItemStockHandler>();
            services.AddScoped<IUpdateRemainingStockAmountHandler, UpdateRemainingStockAmountHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HomeInventoryDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            //TODO: Changing culture: https://www.mikesdotnetting.com/article/345/localisation-in-asp-net-core-razor-pages-cultures
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
