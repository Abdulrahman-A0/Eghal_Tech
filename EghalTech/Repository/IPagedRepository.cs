using EghalTech.Models;
using System.Linq.Expressions;
using X.PagedList;

namespace EghalTech.Repository
{
    public interface IPagedRepository<T> : IRepository<T>
    {
        IPagedList<T> GetPaged(
            int page,
            int pageSize = 10,
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[] includes
        );
    }
}
