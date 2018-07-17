using System;

using Microsoft.Extensions.Configuration;
using System.Linq;
using Health.Web.Data;
using Health.Web.Models;
using Microsoft.AspNetCore.Mvc;

using System.Web;
using Microsoft.AspNetCore.Http;

namespace Health.Web.Controllers
{
    public class AdminController : BaseController
    {
        private IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if (HttpContext.Request.Cookies["User"] != null)
            {
                ViewBag.UserInfo = GetUserInformation(Convert.ToInt32(HttpContext.Request.Cookies["User"].ToString()));
                return View();
            }
            else
            {
                if (HttpContext.Request.Method == "POST")
                {
                    int usrid=Login(email, password);
                    ViewBag.UserInfo = GetUserInformation(usrid);
                    return View();
                }
                return View("Login");
            }
        }

        //[HttpPost]
        public int Login(string email, string password)
        {
            var dbFactory = new HealthDataContextFactory(
               dataProvider: LinqToDB.DataProvider.MySql.MySqlTools.GetDataProvider(),
                connectionString: _configuration.GetConnectionString("Health")
           );
            using (var context = dbFactory.Create())
            {
                IQueryable<User> userQuery =
                    from users in context.Users
                        //where user.Surname == username and user.Password==password
                    select users;


                var usr = context.Users.SingleOrDefault(u => u.Email == email);
                if (usr.Admin && usr.Password == password)
                {
                    SetCookie("User", usr.Id.ToString(), 20);
                    //HttpContext.Session.Set("User", Encoding.Unicode.GetBytes(usr.Id.ToString()));
                    return usr.Id;

                    //return View("Index");
                }
            }
            return 0;
            //return View("Login");

            /*int userId = user.Id;
            string name = user.Name;
            string surname = user.Surname;
            string email = user.Email;

            return View();*/
        }
        public void SetCookie(string key, string value, int? expireTime)
        {
            
            
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                option.Secure = true;
                option.IsEssential = true;
            }
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
            //Response.Cookies["health"][key].Value=value;
        }

        public User GetUserInformation(int id)
        {
            var dbFactory = new HealthDataContextFactory(
             dataProvider: LinqToDB.DataProvider.MySql.MySqlTools.GetDataProvider(),
              connectionString: _configuration.GetConnectionString("Health")
            );
            using (var context = dbFactory.Create())
            {
                IQueryable<User> userQuery =
                    from users in context.Users
                    where users.Id == id
                    select users;

                var usr = context.Users.First();
                return usr;
            }
        }
    }
}
