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
        string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");

        string result = ImportCategoryProducts(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        ImportCategoriesProductDTO[] categoryProductDTOs = JsonConvert.DeserializeObject<ImportCategoriesProductDTO[]>(inputJson)!;
        IMapper mapper = MapperProvider();
        CategoryProduct[] categoryProducts = mapper.Map<CategoryProduct[]>(categoryProductDTOs);

        context.CategoriesProducts.AddRange(categoryProducts);  
        context.SaveChanges();

        return $"Successfully imported {categoryProductDTOs.Count()}"; 
    }
}