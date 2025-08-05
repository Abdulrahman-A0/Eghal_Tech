using EghalTech.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.Extensions;

namespace EghalTech.Repository
{
    public class PagedRepository<T> : Repository<T>, IPagedRepository<T> where T : class
    {
        public PagedRepository(AppDbContext _context) : base(_context)
        {
        }

        public IPagedList<T> GetPaged(
                int page,
                int pageSize = 10,
                Expression<Func<T, bool>>? filter = null,
                params Expression<Func<T, object>>[] includes
            )
        {
            var query = dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            foreach (var item in includes)
                query.Include(item);

            return query.ToPagedList(page, pageSize);
        }
    }
}
