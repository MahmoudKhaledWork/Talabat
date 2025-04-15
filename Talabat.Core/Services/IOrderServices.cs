using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderServices
    {
        public Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress);
        public Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail);
        public Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail , int OrderId);
        public Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync();
    }
}
