using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Core.Entities;

namespace Product.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            builder.HasData(
                new Products  {Id = 1, Name="Pro 1", Description="des 1", Price =100, CategoryId=1},
                new Products { Id = 2, Name="Pro 2", Description="des 2", Price =120, CategoryId=2 },
                new Products { Id = 3, Name="Pro 3", Description="des 3", Price =130, CategoryId=3 },
                new Products { Id = 4, Name="Pro 3", Description="des 3", Price =130, CategoryId=2 }
                );
        }
    }
}
