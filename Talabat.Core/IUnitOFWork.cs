using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;

namespace Talabat.Core
{
    public interface IUnitOFWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository <T> () where T :BaseEntity;
        Task<int> CompleteAsync(); 
    }
}
