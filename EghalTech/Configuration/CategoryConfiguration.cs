using EghalTech.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EghalTech.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("nVarChar(50)");

            builder.HasData(CategoryData());
        }

        private List<Category> CategoryData()
        {
            return new List<Category>()
            {
                new Category {Id=1, Name = "Laptops" },
                new Category {Id=2, Name = "Smartphones" },
                new Category {Id=3, Name = "Accessories" },
                new Category {Id=4, Name = "Audio Devices" }
            };
        }
    }
}
