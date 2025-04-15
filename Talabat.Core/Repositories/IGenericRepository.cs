using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        Task Delete(T entity);
        Task Update(T entity);
        Task Add(T entity);
        #region Without Specification 
        //public Task<IEnumerable<T>> GetAllAsync();
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specification 

        //Task<IEnumerable<T>> GetAllAsyncWithSpec (ISpecification<T> Spec);
        Task<IReadOnlyList<T>> GetAllAsyncWithSpec (ISpecification<T> Spec);
        Task<T> GetByIdAsyncWithSpec(ISpecification<T> Spec);

        Task<int> GetCountWithSpecAsync(ISpecification<T> Spec);

        #endregion
        
        


    }
}
