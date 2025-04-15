using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbcontext)
        {
            if (!dbcontext.ProductBrands.Any())
            {
                //var BrandData = File.ReadAllText("C:\\Users\\lenovo\\source\\repos\\TalabatAPIs.sol\\Talabat.Repository\\DataSeed\\brands.json");
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbcontext.Set<ProductBrand>().AddAsync(Brand);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            // Seed Types 
            if (!dbcontext.ProductTypes.Any())
            {

                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductTypes>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                    {
                        await dbcontext.Set<ProductTypes>().AddAsync(Type);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            // Seeding Product
            if (!dbcontext.Products.Any())
            {

                var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                    {
                        await dbcontext.Set<Product>().AddAsync(Product);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }

            if (!dbcontext.DeliveryMethods.Any())
            {
                var DelivreyMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DelivreyMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DelivreyMethodsData);
                if (DelivreyMethods?.Count > 0)
                {
                    foreach (var DelivreyMethod in DelivreyMethods)
                    {
                        await dbcontext.Set<DeliveryMethod>().AddAsync(DelivreyMethod);
                    }
                }
            }

             await dbcontext.SaveChangesAsync();
        }
    }
}
