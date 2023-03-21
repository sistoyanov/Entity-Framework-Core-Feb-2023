using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        Console.WriteLine(GetSoldProducts(context));

    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetSoldProducts(ProductShopContext context)
    {
        // IMapper mapper = MapperProvider();

        //ExportUserDTO[] usersDTO = context.Users
        //     .AsNoTracking()
        //     //.Include(p => p.ProductsSold)
        //     .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
        //     .OrderBy(u => u.LastName)
        //     .ThenBy(u => u.FirstName)
        //     .ToArray()
        //     .Where(u => u.)
        //     .ProjectTo<ExportUserDTO>(mapper.ConfigurationProvider)
        //     .ToArray();

        var usersDTO = context.Users
             .AsNoTracking()
             .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
             .OrderBy(u => u.LastName)
             .ThenBy(u => u.FirstName)
             .Select(u => new 
             {
                 firstName = u.FirstName,
                 lastName = u.LastName,
                 soldProducts = u.ProductsSold
                       .Where(p => p.Buyer != null)
                       .Select(p => new
                       {
                           name = p.Name,
                           price = p.Price,
                           buyerFirstName = p.Buyer.FirstName,
                           buyerLastName = p.Buyer.LastName
                       })
                       .ToArray()
             })
             .ToArray();

        return JsonConvert.SerializeObject(usersDTO, Formatting.Indented);
    }
}