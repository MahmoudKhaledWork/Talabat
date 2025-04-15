using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public class ProductTypesBrandSpecification : BaseSpecification<Product>
    {
        //  Ctor For GetALL Products 
        public ProductTypesBrandSpecification() : base()
        {
            Includes.Add(P => P.ProudctBrand);
            Includes.Add(P => P.ProductType);
        }
        //public ProductTypesBrandSpecification(string sort, int? BrandId, int? TypeId) 
        public ProductTypesBrandSpecification(ProductSpecParams Params)
        : base
            (p => 
            (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId)
            &&
            (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
            )
        {
            Includes.Add(P => P.ProudctBrand);
            Includes.Add(P => P.ProductType);

             if (!string.IsNullOrEmpty(Params.sort))
            {
                switch (Params.sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;

                }
            }

            // Products  = 100 
            // PageSize  = 10 
            // PageIndex = 5
            // Skip = 40 => 10 * 4


            // ApplyPagination(Skip , Take )
            // Skip = PageSize (10) * PageIndex (5) - 1 
            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);




        }
        public ProductTypesBrandSpecification(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProudctBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
