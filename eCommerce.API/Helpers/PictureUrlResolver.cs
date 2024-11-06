using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eCommerce.API.DTOs;
using eCommerce.Core.Entities;

namespace eCommerce.API.Helpers
{
    public class PictureUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        public IConfiguration _configuration { get; }
        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            return "";
        }
    }
}