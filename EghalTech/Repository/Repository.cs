using EghalTech.Data;
using Microsoft.EntityFrameworkCore;

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

        public T GetById(int id)
        {
            return dbSet.Find(id);
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
