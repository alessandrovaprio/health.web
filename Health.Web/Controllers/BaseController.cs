using Health.Web.Data;
using Health.Web.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}