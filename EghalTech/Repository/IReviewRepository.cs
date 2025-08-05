using EghalTech.Models;

namespace EghalTech.Repository
{
    public interface IReviewRepository : IRepository<Review>
    {
        List<Review> GetByProductId(int productId);
    }
}
