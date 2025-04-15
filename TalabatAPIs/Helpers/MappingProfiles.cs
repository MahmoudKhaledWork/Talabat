using AutoMapper;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Entites.Order_Aggregate;
using TalabatAPIs.DTOS;
using IdentityAddress = Talabat.Core.Entites.Identity.Address;
using OrderAddress = Talabat.Core.Entites.Order_Aggregate.Address;
namespace TalabatAPIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
           .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
           .ForMember(d => d.ProudctBrand, o => o.MapFrom(s => s.ProudctBrand.Name))
           .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>())
           .ReverseMap();
            
            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
            CreateMap<OrderAddress, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(O=>O.DelivreyMethod , M=>M.MapFrom(S=>S.DelivreyMethod.ShortName))
                .ForMember(O=>O.DelivreyMethodCost , M=>M.MapFrom(S=>S.DelivreyMethod.Cost))
                .ReverseMap();

            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(O=>O.ProductId , M=>M.MapFrom(S=>S.Product.ProductId))
                .ForMember(O=>O.ProductName , M=>M.MapFrom(S=>S.Product.ProductName))
                .ForMember(O=>O.PictureUrl , M=>M.MapFrom(S=>S.Product.PictureUrl))
                .ForMember(O=>O.PictureUrl , M=>M.MapFrom<OrderITemPictureUrlResolver>())
                .ReverseMap();


            CreateMap<BasketItemsDto, BasketItem>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();



        }
    }
}
