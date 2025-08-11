using EghalTech.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EghalTech.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext context;
        protected readonly DbSet<T> dbSet;

        public Repository(AppDbContext _context)
        {
            context = _context;
            dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(int id)
        {
            dbSet.Remove(GetById(id));
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            foreach (var include in includes)
                query = query.Include(include);

            return query.FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}
