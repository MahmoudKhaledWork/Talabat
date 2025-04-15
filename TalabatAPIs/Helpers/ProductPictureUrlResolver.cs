using AutoMapper;
using AutoMapper.Execution;
using Talabat.Core.Entites;
using TalabatAPIs.DTOS;

namespace TalabatAPIs.Helpers
{
    // Source , Destnation , Datatype => <Product, ProductToReturnDTO, string>
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            }            
            return string.Empty;                
        }
    }
}
