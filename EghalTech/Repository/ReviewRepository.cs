using EghalTech.Data;
using EghalTech.Models;

namespace EghalTech.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext _context) : base(_context)
        {
        }

        public List<Review> GetByProductId(int productId)
        {
            return context.Reviews.Where(r => r.ProductId == productId)
                .ToList();
        }
    }
}
