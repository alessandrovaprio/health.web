using System;
namespace Health.Web.Models
{
    public interface IHasId<T>
    {
        T Id { get; set; }
    }
}
