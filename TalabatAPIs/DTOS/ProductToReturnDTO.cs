using Talabat.Core.Entites;

namespace TalabatAPIs.DTOS
{
    public class ProductToReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }        
        //public ProductBrand ProudctBrand { get; set; }
        public string ProudctBrand { get; set; }
        
        public int ProductTypeId { get; set; }
        //public ProductTypes ProductType { get; set; }
        public string ProductType { get; set; }
    }
}
