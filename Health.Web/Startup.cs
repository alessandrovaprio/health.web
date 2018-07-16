using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Health.Web.Models;
using LinqToDB.DataProvider;
using Pomelo.EntityFrameworkCore.MySql;
using Health.Web.Data;
using Swashbuckle;
using Health.Web.Extensions;
using LinqToDB;
using LinqToDB.DataProvider.SQLite;

namespace Health.Web
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
            /*services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });*/

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);



            var dbFactory = new HealthDataContextFactory(
                dataProvider: LinqToDB.DataProvider.MySql.MySqlTools.GetDataProvider(),
                connectionString: Configuration.GetConnectionString("Health")
            );

            services.AddSingleton<IDataContextFactory<HealthDataContext>>(dbFactory);
            SetupDatabase(dbFactory);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Oops, something went wrong");
            });
        }

        void SetupDatabase(IDataContextFactory<HealthDataContext> dataContext)
        {
            using (var db = dataContext.Create())
            {
                
                db.CreateTableIfNotExists<User>();
                /*db.CreateTableIfNotExists<FitbitDevice>();
                db.CreateTableIfNotExists<Heartbeat>();*/

                  db.InsertOrReplace(new User { Id = 1, Name="Alessandro",Surname="Vaprio",Email="alessandro.vaprio@gmail.com",Password="Vprlsn90",Admin=true,Doctor=true,Timestamp = DateTime.Now });
                /*db.InsertOrReplace(new FitbitDevice { Id = "D1", AccountId = "A1" });
                db.InsertOrReplace(new FitbitDevice { Id = "D2", AccountId = "A2" });

                db.Insert(new Heartbeat { DeviceId = "D1", Timestamp = DateTime.Now, Value = 60 });
                db.Insert(new Heartbeat { DeviceId = "D1", Timestamp = DateTime.Now, Value = 80 });
                db.Insert(new Heartbeat { DeviceId = "D1", Timestamp = DateTime.Now, Value = 65 });
                db.Insert(new Heartbeat { DeviceId = "D2", Timestamp = DateTime.Now, Value = 60 });
                db.Insert(new Heartbeat { DeviceId = "D2", Timestamp = DateTime.Now, Value = 70 });

                db.InsertOrReplace(new Category { Id = 1, Name = "Band" });
                db.InsertOrReplace(new Category { Id = 2, Name = "Smartwatch" });*/
            }
        }
    }
}
