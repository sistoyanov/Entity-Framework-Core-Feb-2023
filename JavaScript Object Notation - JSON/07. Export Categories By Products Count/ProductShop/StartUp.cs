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
        Console.WriteLine(GetCategoriesByProductsCount(context));

    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {

        var categories = context.Categories
            .AsNoTracking()
            .OrderByDescending(c => c.CategoriesProducts.Count())
            .Select(c => new
            {
                category = c.Name,
                productsCount = c.CategoriesProducts.Count(),
                averagePrice = c.CategoriesProducts.Average(cp => cp.Product.Price).ToString("f2"),
                totalRevenue = c.CategoriesProducts.Sum(cp => cp.Product.Price).ToString("f2")
            })
            .ToArray();

        return JsonConvert.SerializeObject(categories, Formatting.Indented);
    }
}