using EghalTech.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EghalTech.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("nVarChar(100)");

            builder.HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.WishList)
                .WithOne(w => w.User)
                .HasForeignKey<WishList>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
