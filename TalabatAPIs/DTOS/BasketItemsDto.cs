using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.DTOS
{
    public class BasketItemsDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "The Price Cant Be Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Should Have At Least One Item")]
        public int Quantity { get; set; }
    }
}