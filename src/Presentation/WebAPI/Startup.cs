using HomeInv.Business;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.Settings;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace WebAPI
{
    public class Startup
    {
        public IWebHostEnvironment Env { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection()
                    .SetApplicationName($"expense-tracker-{Env.EnvironmentName}")
                    .PersistKeysToFileSystem(new DirectoryInfo($@"{Env.ContentRootPath}\keys"))
                    .SetDefaultKeyLifetime(System.TimeSpan.FromDays(1000));
            //.DisableAutomaticKeyGeneration(); -> https://stackoverflow.com/a/43327546/837560

            services.AddDbContext<HomeInventoryDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //services.AddDefaultIdentity<IdentityUser>(options =>
            //{
            //    options.SignIn.RequireConfirmedAccount = true;
            //    options.Password.RequireDigit = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.User.RequireUniqueEmail = true;
            //})
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<HomeInventoryDbContext>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddOptions();

            services.Configure<CosmosSettings>(Configuration.GetSection("CosmosSettings"));

            ConfigureHomeInventoryServices(services);
            ConfigureIdentityServices(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
        }

        void ConfigureHomeInventoryServices(IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IUserService, UserService>();
        }

        void ConfigureIdentityServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-5.0
            /// https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/security/authentication/cookie/samples/3.x/CookieSample
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/social-without-identity?view=aspnetcore-5.0
            /// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-5.0

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //{
            //    options.Cookie.Expiration = TimeSpan.FromDays(30);
            //    //options.Cookie.MaxAge = TimeSpan.FromDays(120);
            //    options.SlidingExpiration = true;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
