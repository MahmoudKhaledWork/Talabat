using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery( IQueryable<T> InputQuery , ISpecification<T> specification)
        {
            var Query = InputQuery; //_dbcontext.Products
            if (specification.Criteria is not null)
            {
                //_dbcontext.Products.Where(P => P.Id == id)
                Query = Query.Where(specification.Criteria);
            }
            if (specification.OrderBy is not null)
            {
                Query = Query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(specification.OrderByDescending);
            }
            if(specification.IsPaginationEnabled)
            {
                Query = Query.Skip(specification.Skip).Take(specification.Take);
            }
            Query = specification.Includes.Aggregate(Query, (CurrentQuery, IncludeExpressions) => CurrentQuery.Include(IncludeExpressions));

            return Query;
        }
        // Function to build Query 
        // _dbcontext.Products
        // .Where(P => P.Id == id)
        // .Include(P => P.ProudctBrand).Include(P => P.ProductType)


        //Filtration in app 
        //public static IEnumerable<T>

        // Filtration in DB
        //public static IQueryable<T> 


        // _dbcontext.Products.Where(P => P.Id == id).Include(P => P.ProudctBrand).Include(P => P.ProductType)




    }
}
