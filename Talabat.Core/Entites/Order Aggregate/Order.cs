using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod delivreyMethod, ICollection<OrderItem> items, decimal subTotal , string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DelivreyMethod = delivreyMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }


        //public int DelivreyMethodId { get; set; }
        // Nav Property "one"  
        public DeliveryMethod DelivreyMethod { get; set; }

        // Nav Property "Many"
        public ICollection<OrderItem> Items { get; set; } 
            = new HashSet<OrderItem>();

        public decimal SubTotal { get; set; }

        //[NotMapped] // Drived Attribute not mapped To Database 
        //public decimal Total {get => SubTotal + DelivreyMethod.Cost ;}
        public decimal GetTotal()        
        => SubTotal + DelivreyMethod.Cost;
        
        public string PaymentIntentId { get; set; } 
    }
}
