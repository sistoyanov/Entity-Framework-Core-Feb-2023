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

        string result = GetCategoriesByProductsCount(context);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

        return config.CreateMapper();
    }

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        IMapper mapper = CreateMapper();

        ExportCategoryDTO[] categories = context.Categories
            .AsNoTracking()
            .OrderByDescending(c => c.CategoryProducts.Count())
            .ThenBy(c => c.CategoryProducts.Sum(cp => cp.Product.Price))
            .ProjectTo<ExportCategoryDTO>(mapper.ConfigurationProvider)
            .ToArray();

        string output = XmlHelper.Serialize(categories, "Categories");
        return output;
    }
}