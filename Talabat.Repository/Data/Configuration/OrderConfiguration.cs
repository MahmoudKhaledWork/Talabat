using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Repository.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Configure That The Order Status :
            // In Database => is a String => Pending 
            // In The App => OrderStatus.Pending 

            builder.Property(O => O.Status)
                   .HasConversion(O => O.ToString(), O => (OrderStatus) Enum.Parse(typeof(OrderStatus), O));

            builder.Property(S => S.SubTotal)
                   .HasColumnType("decimal(18,2)");

            // Dont Make THe Address A Class In Database 
            // Make It Configure In Same Table With The Order 
            builder.OwnsOne(A => A.ShippingAddress, S => S.WithOwner());

            builder.HasOne(O => O.DelivreyMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
