using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Core.Entities;

namespace Product.Infrastructure.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(50);

            builder.HasData(
            new Category { Id = 1, Name= "Category 1", Description="asd" },
            new Category { Id = 2, Name= "Category 2", Description="asd 2" },
            new Category { Id = 3, Name= "Category 3", Description="asd 3" });
        }
    }
}
