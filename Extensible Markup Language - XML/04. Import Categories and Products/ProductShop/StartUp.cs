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
        string inputXml = File.ReadAllText(@"../../../Datasets/categories-products.xml");

        string result = ImportCategoryProducts(context, inputXml);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

        return config.CreateMapper();
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();

        ImportCategoryProductDTO[] categoryProductDTOs = XmlHelper.Deserialize<ImportCategoryProductDTO[]>(inputXml, "CategoryProducts");
        ICollection<CategoryProduct> categoryProducts= new HashSet<CategoryProduct>();

        foreach (var categoryProduct in categoryProductDTOs)
        {
            if (context.CategoryProducts.Any(cp => cp.CategoryId != categoryProduct.CategoryId) ||
                context.CategoryProducts.Any(cp => cp.ProductId != categoryProduct.ProductId))
            {
                continue;
            }

            CategoryProduct newCategoryProduct = mapper.Map<CategoryProduct>(categoryProduct);
            categoryProducts.Add(newCategoryProduct);
        }

        context.CategoryProducts.AddRange(categoryProducts);
        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Count}";
    }
}