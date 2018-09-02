using System;

using Microsoft.Extensions.Configuration;
using System.Linq;
using Health.Web.Data;
using Health.Web.Models;
using Microsoft.AspNetCore.Mvc;

using System.Web;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LinqToDB;

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

            User userModel = new User();
            //valido il campo mail con regex e inserisco errore nello state model
            string errValidateEmail = userModel.ValidateEmail(email);
            if(errValidateEmail!=""){
                ModelState.AddModelError("Email", errValidateEmail);
            }
            //se ci sono errori di validazione torna alla login con visualizzazione errore
            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            if (HttpContext.Request.Cookies["User"] != null)
            {
                ViewBag.UserInfo = GetUserInformation(Convert.ToInt32(HttpContext.Request.Cookies["User"].ToString()),_configuration);
                return View();
            }
            else
            {
                if (HttpContext.Request.Method == "POST")
                {
                    int usrid=Login(email, password);
                    if(usrid!=0){
                        ViewBag.UserInfo = GetUserInformation(usrid,_configuration);
                        return View();    
                    }

                }
                //se i parametri inseriti non restituiscono un utente, si ritorna un errore e la pagina di login
                ModelState.AddModelError("Password", "Utente o pasword non corrette");
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
                var userquery = (from u in context.Users
                             where u.Admin == true && u.Email == email && u.Password == password
                                   
                                   select u).Take(1);
                

                foreach (var u in userquery)
                {
                    SetCookie("User", u.Id.ToString(), 20);
                    //HttpContext.Session.Set("User", Encoding.Unicode.GetBytes(usr.Id.ToString()));
                    return u.Id;
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

       

        //---------------------------------------------------------------------------------

        public IActionResult ShowUsers(){
            MyDataContext dataContext = new MyDataContext();
            var context = (IDataContextFactory<HealthDataContext>)dataContext.GetMyDataContextContext(_configuration);
            //ViewBag.ListOfUsers = GetAllusers(context);
            return View("Show",GetAllusers(context));
        }

        public List<User> GetAllusers(IDataContextFactory<HealthDataContext> dbfactory)
        {
            List<User> users = new List<User>();
            using (var context = dbfactory.Create())
            {

                users = context.Users.ToList();
            }
            return users;
        }



        public bool WriteUserToDB(User user)
        {
            MyDataContext dataContext = new MyDataContext();
            var context = (IDataContextFactory<HealthDataContext>)dataContext.GetMyDataContextContext(_configuration);

            using (var db = context.Create())
            {
                var lastidquery = (from u in db.Users
                                       //where t.DeliverySelection == true && t.Delivery.SentForDelivery == null
                                   orderby u.Id descending
                                   select u).Take(1);
                int id = 0;

                foreach (var u in lastidquery)
                {
                    id = u.Id + 1;
                }
                if (id != 0)
                {
                    user.Id = id;
                    if (db.Insert(user) > 0)
                    {
                        return true;
                    }
                }


            }

            return false;
        }

    }
}
