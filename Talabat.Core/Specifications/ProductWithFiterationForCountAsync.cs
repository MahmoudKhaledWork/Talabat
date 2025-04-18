﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public class ProductWithFiterationForCountAsync : BaseSpecification<Product>
    {
        public ProductWithFiterationForCountAsync(ProductSpecParams Params)    
        : base
        (p =>
        (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
        &&
        (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId)
        &&
        (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
        )
        {
            
        }
    }
}
