using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Repository.Data.Configuration
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductTypes>
    {
        public void Configure(EntityTypeBuilder<ProductTypes> builder)
        {
            builder.Property(T => T.Name).IsRequired();
        }
    }
}
