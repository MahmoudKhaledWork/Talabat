using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GeneriecRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GeneriecRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #region Sepcification Design Pattern 

        // Should Be Static : 
        //_dbcontext.Products   => Entry Point 

        // Should Be Dynamic
        //.Where(p=>p.Id == id) => Where Condtion 
        //.Include(P => P.ProudctBrand).Include(P => P.ProductType) => List of Includes

        #endregion
        #region Without
        //public async Task<IEnumerable<T>> GetAllAsync()
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            ///if (typeof(T) == typeof(Product))
            ///{
            ///    return (IReadOnlyList<T>)await _dbcontext.Products.Include(P => P.ProudctBrand).Include(P => P.ProductType).ToListAsync();
            ///}
            ///else
                return await _dbcontext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);

            //=> await _dbcontext.Set<T>().Where(T => T.Id == id).FirstOrDefaultAsync();
            // return await _dbcontext.Products.Where(P => P.Id == id).Include(P => P.ProudctBrand).Include(P => P.ProductType);
        }

        #endregion
        //public async Task<IEnumerable<T>> GetAllAsyncWithSpec(ISpecification<T> Spec)
        public async Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecification<T> Spec)
        {
            // _dbcontext.Products.Include(P => P.ProudctBrand).Include(P => P.ProductType)
            //return await SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), Spec).ToListAsync();
            return await ApplySpecification(Spec).ToListAsync();
        }
        public async Task<T> GetByIdAsyncWithSpec(ISpecification<T> Spec)
        {
            //return await SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), Spec).FirstOrDefaultAsync();
            return await ApplySpecification(Spec).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpecification (ISpecification<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpecification(Spec).CountAsync();
        }

        public async Task Add(T entity)        
        => await _dbcontext.Set<T>().AddAsync(entity);

        public async Task Delete(T entity)
        =>  _dbcontext.Set<T>().Remove(entity);

        public async Task Update(T entity)
        =>  _dbcontext.Set<T>().Update(entity);
      
    }
}
