using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eCommerce.API.DTOs;
using eCommerce.Core.Entities;
using eCommerce.Core.Entities.Order_Aggregation;

namespace eCommerce.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver>());


            CreateMap<AddressDTO, Address>();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d=>d.Id, O=> O.MapFrom(S=> S.Product.Id))
                .ForMember(d=>d.Name, O=> O.MapFrom(S=> S.Product.Name))
                .ForMember(d=>d.PrictureUrl,O=>O.MapFrom(S=>S.Product.PrictureUrl))
                .ForMember(d=>d.PrictureUrl, O=> O.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<BasketItem, BasketItemDTO>();
            CreateMap<CustomerBasket, CustomerBasketDTO>();
        }
    }
}