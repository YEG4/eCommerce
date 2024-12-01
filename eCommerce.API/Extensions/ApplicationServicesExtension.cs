using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.API.Helpers;
using eCommerce.Core.Repositories;
using eCommerce.Repository;
using eCommerce.Repository.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace eCommerce.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfiles));
            return services;
        }
    }
}