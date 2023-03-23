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

        string result = GetSoldProducts(context);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

        return config.CreateMapper();
    }

    public static string GetSoldProducts(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();

        ExportUserDTO[] users = context.Users
            .AsNoTracking()
            .Where(u => u.ProductsSold.Count() > 0)
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Take(5)
            .ProjectTo<ExportUserDTO>(mapper.ConfigurationProvider)
            .ToArray();

        string output = XmlHelper.Serialize(users, "Users");
        return output;
    }
}