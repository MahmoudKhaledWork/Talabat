using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderWithPaymentSpec : BaseSpecification<Order>
    {
        public OrderWithPaymentSpec(string PaymentIntentId) : base(P => P.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
