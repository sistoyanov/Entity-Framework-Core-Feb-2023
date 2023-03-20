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
        string inputJson = File.ReadAllText(@"../../../Datasets/products.json");

        string result = ImportProducts(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        ImportProductDTO[] productDTOs = JsonConvert.DeserializeObject<ImportProductDTO[]>(inputJson);
        IMapper mapper = MapperProvider();
        Product[] products = mapper.Map<Product[]>(productDTOs);

        context.Products.AddRange(products);  
        context.SaveChanges();

        return $"Successfully imported {products.Length}"; 
    }
}