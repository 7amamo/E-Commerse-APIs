using Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure.Data.Configuration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SupTotal).HasColumnType("decimal(18,2)");
            builder.Property(O => O.Status).HasConversion(OStaus => OStaus.ToString(), OStatu => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatu));

            builder.OwnsOne(o => o.ShippingAddress, SA => SA.WithOwner());

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
