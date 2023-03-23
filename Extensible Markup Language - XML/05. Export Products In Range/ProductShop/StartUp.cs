using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();

        string result = GetProductsInRange(context);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

        return config.CreateMapper();
    }

    public static string GetProductsInRange(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();

        var buytest = context.Products.ToArray();

        ExportProductDTO[] products = context.Products
            .AsNoTracking()
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Take(10)
            .ProjectTo<ExportProductDTO>(mapper.ConfigurationProvider)
            .ToArray();

        string output = XmlHelper.Serialize(products, "Products");
        return output;
    }
}