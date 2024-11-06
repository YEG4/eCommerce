using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Core.Entities;

namespace eCommerce.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product>
    {
        /* Get All Products Endpoint */
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams specParams)
        : base(P =>
        (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId)
        &&
        (!specParams.TypeId.HasValue || P.ProductTypeId == specParams.TypeId)
        )
        {
            Includes.Add(x => x.ProductBrand);
            Includes.Add(x => x.ProductType);
            if (!string.IsNullOrWhiteSpace(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceasc":
                        OrderBy = P => P.Price;
                        break;
                    case "pricedesc":
                        OrderByDesc = P => P.Price;
                        break;
                    case "name":
                        OrderBy = P => P.Name;
                        break;
                }
            }

            ApplyPagniation((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        }

        /* Get Product By Id Endpoint*/
        public ProductWithBrandAndTypeSpecifications(int id) : base(x => x.Id == id)
        {
            Includes.Add(x => x.ProductBrand);
            Includes.Add(x => x.ProductType);
        }
    }
}