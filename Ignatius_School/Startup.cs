using Ignatius_School.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CSE6581.Hotel.ATR
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
            //services.Configure<ApiEndpoint>(Configuration.GetSection("ApiEndpoint"));

            services.AddControllersWithViews();




            //services.AddControllers(options =>
            //{
            //    options.Filters.Add<CustomExceptionFilter>();

            //});
            //services
            //    .AddMvc(options =>
            //    {
            //        options.Filters.Add<AddCustomHeaderResourceFilter>();
            //    })
            //    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);


            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportCulture = new[]
                {
                        new CultureInfo("kk-KZ"),
                        new CultureInfo("ru-RU"),
                        new CultureInfo("en-US")
                    };

                options.DefaultRequestCulture = new RequestCulture("kk-KZ", "kk-KZ");
                options.SupportedCultures = supportCulture;
                options.SupportedUICultures = supportCulture;
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341/")
                .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddSingleton<Serilog.ILogger>(Log.Logger);

            services.AddSession(options =>
            {
                options.Cookie.Name = ".Hotel.ATR";
                options.IdleTimeout = TimeSpan.FromSeconds(1000);
                //options.Cookie.Expiration = TimeSpan.FromSeconds(10000);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".AspNetCore.HotelAtr.Cookies";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
                    options.SlidingExpiration = true;

                    options.LoginPath = "/Home/Login";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();


            app.Map("/hc", appMap =>
            {
                appMap.Run(async context =>
                {
                    await context.Response.WriteAsync("Hello from middleware");
                });

            });


            var locOptions = app.ApplicationServices
                .GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(locOptions.Value);



            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //http://localhost:52108/
                //controller
                //action

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}