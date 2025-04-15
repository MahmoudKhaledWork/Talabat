using Talabat.Core.Entites.Order_Aggregate;

namespace TalabatAPIs.DTOS
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
                       
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }


        //public int DelivreyMethodId { get; set; }
        // Nav Property "one"  
        public string DelivreyMethod { get; set; }

        public decimal DelivreyMethodCost { get; set; }
        // Nav Property "Many"
        public ICollection<OrderItemDto> Items { get; set; }
            = new HashSet<OrderItemDto>();

        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
