using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        Console.WriteLine(GetProductsInRange(context));

    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetProductsInRange(ProductShopContext context)
    {
        IMapper mapper = MapperProvider();
        
        ExportProductInRangeDTO[] productsDTO = context.Products
             .AsNoTracking()
             .Where(p => p.Price >= 500 && p.Price <=1000)
             .OrderBy(p => p.Price)
             .ProjectTo<ExportProductInRangeDTO>(mapper.ConfigurationProvider)
             .ToArray();

        return JsonConvert.SerializeObject(productsDTO, Formatting.Indented);
    }
}