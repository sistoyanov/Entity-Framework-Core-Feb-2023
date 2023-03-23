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
        string inputXml = File.ReadAllText(@"../../../Datasets/products.xml");

        string result = ImportProducts(context, inputXml);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

        return config.CreateMapper();
    }

    public static string ImportProducts(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();

        ImportProductDTO[] productDTOs = XmlHelper.Deserialize<ImportProductDTO[]>(inputXml, "Products");

        Product[] products = mapper.Map<Product[]>(productDTOs);

        context.Products.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Length}";
    }
}