using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Repository.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasOne(p => p.ProudctBrand)
                   .WithMany()
                   .HasForeignKey(p => p.ProductBrandId);

            builder.HasOne(P => P.ProductType)
                   .WithMany()
                   .HasForeignKey(P => P.ProductTypeId);

            builder.Property(P => P.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(P => P.Description)
                   .IsRequired();

            builder.Property(P => P.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(P => P.PictureUrl)
                   .IsRequired();


        }
    }
}
