using System;
using Health.Web.Models;
using Microsoft.Extensions.Configuration;

namespace Health.Web.Data
{
    public class MyDataContext
    {
        public HealthDataContextFactory GetMyDataContextContext(IConfiguration Configuration)
        {

            var dbFactory = new HealthDataContextFactory(
               dataProvider: LinqToDB.DataProvider.MySql.MySqlTools.GetDataProvider(),
               connectionString: Configuration.GetConnectionString("Health")
            );
            return dbFactory;
        }
    }
}
