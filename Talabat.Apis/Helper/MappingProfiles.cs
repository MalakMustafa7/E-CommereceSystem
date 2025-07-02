using AutoMapper;
using Talabat.Apis.DTOs;
using Talabat.Core.Models;
using Talabat.Core.Models.Identity;
using Talabat.Core.Models.Order_Aggeregate;

namespace Talabat.Apis.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Product,ProductToReturnDTO>()
                     .ForMember(d=>d.ProductBrand, O=>O.MapFrom(s=>s.ProductBrand.Name))
                     .ForMember(d => d.ProductType, O => O.MapFrom(s => s.ProductType.Name))
                     .ForMember(d=>d.PictureUrl , O=>O.MapFrom<ProductPictureURLResolver>());

            CreateMap<Core.Models.Identity.Address, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO , Core.Models.Order_Aggeregate.Address> ().ReverseMap();
            CreateMap<CustomerBasketDTO, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDTO, BasketItem>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                      .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
                       .ForMember(d => d.DelivaryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTO>()
                     .ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
                     .ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName))
                     .ForMember(d => d.PictureUrl, O => O.MapFrom(s => s.Product.PictureUrl))
                     .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureURLResolver>());
        }
    }
}
