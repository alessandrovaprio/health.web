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
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
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
            app.UseSession();

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
                db.DropTable<User>();
                db.CreateTableIfNotExists<User>();
                /*db.CreateTableIfNotExists<FitbitDevice>();
                db.CreateTableIfNotExists<Heartbeat>();*/

                db.InsertOrReplace(new User { Id = 1, Name = "Alessandro", Surname = "Vaprio", Email = "alessandro.vaprio@gmail.com", Password = "Vprlsn90", Admin = true, Doctor = true, RememberMe = false, Timestamp = DateTime.Now });
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
