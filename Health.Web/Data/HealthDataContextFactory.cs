using LinqToDB.DataProvider;
using Health.Web.Models;
using Microsoft.Extensions.Configuration;

namespace Health.Web.Data
{
    public class HealthDataContextFactory : IDataContextFactory<HealthDataContext>
    {
        readonly IDataProvider dataProvider;

        readonly string connectionString;

        public HealthDataContextFactory(IDataProvider dataProvider, string connectionString)
        {
            this.dataProvider = dataProvider;
            this.connectionString = connectionString;
        }

        public HealthDataContext Create() => new HealthDataContext(dataProvider, connectionString);


       


    }


}