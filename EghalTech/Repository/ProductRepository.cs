using EghalTech.Data;
using EghalTech.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.Extensions;

namespace EghalTech.Repository
{
    public class ProductRepository : PagedRepository<Product>, IProductRepository
    {
        private readonly IMemoryCache memoryCache;

        public ProductRepository(AppDbContext _context, IMemoryCache memoryCache) : base(_context)
        {
            this.memoryCache = memoryCache;
        }

        public List<Product> GetFeaturedProducts(int count = 6)
        {
            string cacheKey = $"FeaturedProducts_{count}";

            if (!memoryCache.TryGetValue(cacheKey, out List<Product> cachedProducts))
            {
                int total = context.Products.Count();
                if (total == 0 || count <= 0)
                    return new List<Product>();

                count = Math.Min(count, total);

                var random = new Random();
                var selectedProducts = new List<Product>();
                var usedIndexes = new HashSet<int>();

                while (selectedProducts.Count < count)
                {
                    int skip = random.Next(0, total);
                    if (!usedIndexes.Contains(skip))
                    {
                        var product = context.Products
                                             .Skip(skip)
                                             .Take(1)
                                             .FirstOrDefault();
                        if (product != null)
                        {
                            selectedProducts.Add(product);
                            usedIndexes.Add(skip);
                        }
                    }
                }


                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                };
                memoryCache.Set(cacheKey, selectedProducts, cacheOptions);

                return selectedProducts;
            }

            return cachedProducts;
        }
    }
}
