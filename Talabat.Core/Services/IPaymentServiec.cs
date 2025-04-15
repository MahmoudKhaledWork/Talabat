using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Services
{
    public interface IPaymentServiec
    {
        // Create OR Update The Payment Intent
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
    }
}
