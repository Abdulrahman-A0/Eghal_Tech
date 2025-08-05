using EghalTech.Models;

namespace EghalTech.Repository
{
    public interface IProductRepository : IPagedRepository<Product>
    {
        List<Product> GetFeaturedProducts(int count);
    }
}
