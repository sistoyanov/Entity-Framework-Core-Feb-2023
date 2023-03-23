using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        string inputXml = File.ReadAllText(@"../../../Datasets/categories.xml");

        string result = ImportCategories(context, inputXml);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

        return config.CreateMapper();
    }

    public static string ImportCategories(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();

        ImportCategoryDTO[] categoryDTOs = XmlHelper.Deserialize<ImportCategoryDTO[]>(inputXml, "Categories");

        Category[] categories = mapper.Map<Category[]>(categoryDTOs);

        context.Categories.AddRange(categories);
        context.SaveChanges();

        return $"Successfully imported {categories.Length}";
    }
}