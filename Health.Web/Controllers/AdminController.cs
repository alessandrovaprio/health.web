using System;

using Microsoft.Extensions.Configuration;
using System.Linq;
using Health.Web.Data;
using Health.Web.Models;
using Microsoft.AspNetCore.Mvc;

using System.Text;

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
            if (HttpContext.Request.Cookies["User"] != null)
            {
                return View();
            }
            else { return View("Login"); }
        }

        [HttpPost]
        public ActionResult Login(string email,string password)
        {
            var dbFactory = new HealthDataContextFactory(
               dataProvider: LinqToDB.DataProvider.MySql.MySqlTools.GetDataProvider(),
                connectionString: _configuration.GetConnectionString("Health")
           );
            using(var context= dbFactory.Create()){
                IQueryable<User> userQuery =
                    from users in context.Users
                                             //where user.Surname == username and user.Password==password
                    select users;
                

                var usr = context.Users.SingleOrDefault(u => u.Email == email);
                if(usr.Admin && usr.Password==password){
                    HttpContext.Session.Set("User", Encoding.Unicode.GetBytes(usr.Id.ToString()));
                    ViewBag.Message = usr.Id;

                    return View("Index");
                }
            }

            return View("Login");

            /*int userId = user.Id;
            string name = user.Name;
            string surname = user.Surname;
            string email = user.Email;

            return View();*/
        }
    }
}
