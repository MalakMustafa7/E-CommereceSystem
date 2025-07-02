using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.Apis.DTOs;
using Talabat.Core.Models.Order_Aggeregate;

namespace Talabat.Apis.Helper
{
    public class OrderItemPictureURLResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;
        public OrderItemPictureURLResolver(IConfiguration configuration) {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {

            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["BaseURL"]}{source.Product.PictureUrl}";
                return String.Empty;
        }
    }
}
