using Core.Entities;
using Core.Entities.Order;
using InfraStructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace InfraStructure.Data
{
    public static class StoreContextSeed
    {

        // Seeding
        public static async Task SeedAsync (StoreContext DbContext)
        {
            // seeding Brands
            if (!DbContext.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../InfraStructure/Data/DataSeed/brands.json");
                                                     
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await DbContext.Set<ProductBrand>().AddAsync(Brand);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }


            // seeding Types
            if(!DbContext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../InfraStructure/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                    {
                        await DbContext.Set<ProductType>().AddAsync(Type);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }


            // seeding Product

            if(!DbContext.Products.Any())
            {
                var ProductsData = File.ReadAllText("../InfraStructure/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                    {
                        await DbContext.Set<Product>().AddAsync(Product);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }


            // seeding DeliveryMethod

            if (!DbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../InfraStructure/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);

                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        await DbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }


        }
    }
}
