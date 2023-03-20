using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        string inputJson = File.ReadAllText(@"../../../Datasets/categories.json");

        string result = ImportCategories(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        ImportCatergoryDTO[] categoryDTOs = JsonConvert.DeserializeObject<ImportCatergoryDTO[]>(inputJson)!.Where(c => c.Name != null).ToArray();
        IMapper mapper = MapperProvider();
        Category[] categories = mapper.Map<Category[]>(categoryDTOs);

        context.Categories.AddRange(categories);  
        context.SaveChanges();

        return $"Successfully imported {categories.Count()}"; 
    }
}