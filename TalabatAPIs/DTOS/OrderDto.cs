using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entites.Order_Aggregate;

namespace TalabatAPIs.DTOS
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }

        public int DeliveryMethodId { get; set; }
                
        public AddressDto ShippingAddress { get; set; }
    }
}
