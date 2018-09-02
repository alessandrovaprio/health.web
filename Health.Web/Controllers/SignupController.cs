using System;

using Microsoft.Extensions.Configuration;
using System.Linq;
using Health.Web.Data;
using Health.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Health.Web.Extensions;
using System.Web;
using Microsoft.AspNetCore.Http;
using LinqToDB;

namespace Health.Web.Controllers
{
    public class SignupController : BaseController
    {
        private IConfiguration _configuration;

        public SignupController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Index(User user)
        {
            User userModel = new User();
            //valido il campo mail con regex e inserisco errore nello state model
            string errValidateEmail = userModel.ValidateEmail(user.Email);
            if (errValidateEmail != "")
            {
                ModelState.AddModelError("Email", errValidateEmail);
            }
            //se ci sono errori di validazione torna alla login con visualizzazione errore
            if (!ModelState.IsValid)
            {
                return View();
            }

                if (HttpContext.Request.Method == "POST")
                {
                    if(WriteUserToDB(user)){
                        TempData["Success"] = "Registrazione avvenuta corretamente";
                        ViewBag.OprationStatus = "Registrazione avvenuta corretamente";
                    }
                    else{
                        ViewBag.OprationStatus = "Errore in fase di registrazione";
                    }


                }
                return View();

        }

        //----------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            User userModel = new User();
            //valido il campo mail con regex e inserisco errore nello state model
            string errValidateEmail = userModel.ValidateEmail(user.Email);
            if (errValidateEmail != "")
            {
                ModelState.AddModelError("Email", errValidateEmail);
            }
            //se ci sono errori di validazione torna alla login con visualizzazione errore
            if (!ModelState.IsValid)
            {
                return View("Update");
            }

            if (HttpContext.Request.Method == "POST")
            {
                if (UpdateUserToDB(user))
                {
                    TempData["Success"] = "Modifica avvenuta corretamente";
                    ViewBag.OprationStatus = "Modifica avvenuta corretamente";
                }
                else
                {
                    ViewBag.OprationStatus = "Errore in fase di aggiornamento";
                }


            }

            return View("Update");

        }

        //-----------------------------------------------------------------------------------
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
        //----------------------------------------------------------
        public bool UpdateUserToDB(User user)
        {
            MyDataContext dataContext = new MyDataContext();
            var context = (IDataContextFactory<HealthDataContext>)dataContext.GetMyDataContextContext(_configuration);

            using (var db = context.Create())
            {
                if (db.Update(user) > 0)
                    {
                        return true;
                    }

            }

            return false;
        }

        //-------------------------------------------------------------------------------------
       
        public IActionResult ShowUser()
        {
            if (HttpContext.Request.Cookies["User"] != null)
            {
                User usr = GetUserInformation(Convert.ToInt32(HttpContext.Request.Cookies["User"].ToString()),_configuration);

                //ViewBag.UserInfo=usr;
                return View("Update", usr);
            }
            return View("Login");



        }
    }
}
