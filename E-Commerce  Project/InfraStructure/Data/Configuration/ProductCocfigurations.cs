using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InfraStructure.Configurations
{
    public class ProductCocfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.ProductBrand).WithMany().HasForeignKey(p => p.ProductBrandId);

            builder.HasOne(p => p.ProductType).WithMany().HasForeignKey(p => p.ProductTypeId);

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        }
    }
}
