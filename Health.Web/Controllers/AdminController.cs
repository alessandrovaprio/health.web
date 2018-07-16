using System;

using Microsoft.Extensions.Configuration;
using System.Linq;
using Health.Web.Data;
using Health.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Health.Web.Controllers
{
    public class AdminController: BaseController
    {
        private IConfiguration _configuration;

        public AdminController(IConfiguration configuration){
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies["myAuth"] != null)
            {
                return View();
            }
            else { return View("Login"); }
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var dbFactory = new HealthDataContextFactory(
               dataProvider: LinqToDB.DataProvider.MySql.MySqlTools.GetDataProvider(),
                connectionString: _configuration.GetConnectionString("Health")
           );
            using(var context= dbFactory.Create()){
                IQueryable<User> userQuery =
                    from users in context.Users
                    where user.Surname == "alessandro.vaprio@gmail.com" 
                    select users;
                
                string username = "alessandro.vaprio@gmail.com";
                var usr = context.Users.SingleOrDefault(u => u.Email == username);
                if(usr.Admin){
                    return View("Index");
                }
            }

            return View();

            /*int userId = user.Id;
            string name = user.Name;
            string surname = user.Surname;
            string email = user.Email;

            return View();*/
        }
    }
}
