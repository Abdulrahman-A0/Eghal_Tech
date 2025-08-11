using System.Linq.Expressions;

namespace EghalTech.Repository
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        List<T> GetAll();
        void Delete(int id);
        T GetById(int id, params Expression<Func<T, object>>[] includes);
        void SaveChanges();
    }
}
