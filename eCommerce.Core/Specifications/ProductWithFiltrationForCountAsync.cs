using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Core.Entities;

namespace eCommerce.Core.Specifications
{
    public class ProductWithFiltrationForCountAsync : BaseSpecifications<Product>
    {
        public ProductWithFiltrationForCountAsync(ProductSpecParams specParams)
         : base(P =>
        (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId)
        &&
        (!specParams.TypeId.HasValue || P.ProductTypeId == specParams.TypeId)
        )
        {

        }
    }
}