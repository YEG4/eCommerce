using System.Text.Json;
using eCommerce.Core.Entities;

namespace eCommerce.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            /* Check first if Database has any records, if not populate Table with initial values. */
            if (!dbContext.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../eCommerce.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(brand);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            /* Check first if Database has any records, if not populate Table with initial values. */
            if (!dbContext.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../eCommerce.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if (types?.Count > 0)
                {
                    foreach (var type in types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(type);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            /* Check first if Database has any records, if not populate Table with initial values. */
            if (!dbContext.Products.Any())
            {
                var productsData = File.ReadAllText("../eCommerce.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await dbContext.Set<Product>().AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

        }
    }
}