using System.Linq;
using Health.Web.Data;
using Health.Web.Models;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Health.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        IDataContextFactory<HealthDataContext> dbFactory;
        HealthDataContext db;

        protected T TryResolve<T>() => (T)HttpContext.RequestServices.GetService(typeof(T));

        protected IDataContextFactory<HealthDataContext> DbFactory => dbFactory ?? (dbFactory = TryResolve<IDataContextFactory<HealthDataContext>>());

        protected HealthDataContext Db => db ?? (db = DbFactory.Create());

        protected override void Dispose(bool disposing)
        {
            db?.Dispose();

            base.Dispose(disposing);
        }
        public User GetUserInformation(int id,IConfiguration _configuration)
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

                foreach (var user in userQuery)
                {
                    return user;
                }

            }
            return null;
        }

        public bool UpdateUserToDB(User user,IConfiguration _configuration)
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
       
    }
}