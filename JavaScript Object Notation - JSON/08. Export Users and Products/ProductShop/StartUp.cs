using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        Console.WriteLine(GetUsersWithProducts(context));

    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetUsersWithProducts(ProductShopContext context)
    {

        var query = context.Users
            .Where(u => u.ProductsSold.Count > 0)
            .OrderByDescending(u => u.ProductsSold.Count(p => p.Buyer != null))
            .Select(u => new 
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                age = u.Age,
                soldProducts = new
                {
                    count = u.ProductsSold.Count(p => p.Buyer != null),
                    products = u.ProductsSold
                       .Where(p => p.Buyer != null)
                       .Select(p => new 
                       {
                          name = p.Name,
                          price = p.Price
                       })
                       .ToArray()
                }
                
            })
            .ToArray();

        var output = new { usersCount = query.Count(), users = query };

        var serializeOptions = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        return JsonConvert.SerializeObject(output, serializeOptions);
    }
}