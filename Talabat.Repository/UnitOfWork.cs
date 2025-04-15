using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOFWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _Repositories;
        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _Repositories = new Hashtable();
        }

        public async Task<int> CompleteAsync()
        => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var Type = typeof(T).Name; // Name Of The Entity 
            if (!_Repositories.ContainsKey(Type))
            {
                var Repository = new GeneriecRepository<T>(_dbContext);
                _Repositories.Add(Type, Repository); // Key , Value 
            }
            return  _Repositories[Type] as IGenericRepository<T>;
        }
    }
}
