using EghalTech.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EghalTech.Configuration
{
    public class WishListItemConfiguration : IEntityTypeConfiguration<WishListItem>
    {
        public void Configure(EntityTypeBuilder<WishListItem> builder)
        {
            builder.HasIndex(wi => new { wi.WishlistID, wi.ProductId })
                .IsUnique();
        }
    }
}
