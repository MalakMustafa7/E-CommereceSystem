using AutoMapper;
using AutoMapper.Execution;
using Talabat.Apis.DTOs;
using Talabat.Core.Models;

namespace Talabat.Apis.Helper
{
    public class ProductPictureURLResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureURLResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
              return $"{_configuration["BaseURL"]}{source.PictureUrl}";
            return String.Empty;
        }
    }
}
