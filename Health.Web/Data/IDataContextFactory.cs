using LinqToDB.Data;

namespace Health.Web.Data
{
    public interface IDataContextFactory<T>
        where T : DataConnection
    {
        T Create();
    }
}