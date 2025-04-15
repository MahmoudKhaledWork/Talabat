using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;
using Order = Talabat.Core.Entites.Order_Aggregate.Order;

namespace Talabat.Core.Specifications.Order_spec
{
    public class OrderSpecification :BaseSpecification<Order>
    {
        public OrderSpecification(string email , int orderId):base(O=>O.BuyerEmail==email && O.Id == orderId)
        {
            Includes.Add(O => O.DelivreyMethod);
            Includes.Add(O => O.Items);           
        }
        public OrderSpecification(string email):base(O=>O.BuyerEmail == email)
        {
            Includes.Add(O => O.DelivreyMethod);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }
        
    }
}
