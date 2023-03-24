using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using ProductShop.Utilities;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
        string inputXml = File.ReadAllText(@"../../../Datasets/suppliers.xml");

        string result = ImportSuppliers(context, inputXml);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

        return config.CreateMapper();
    }

    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();

        ImportSupplierDTO[] supliersDTOs = XmlHelper.Deserialize<ImportSupplierDTO[]>(inputXml, "Suppliers");

        Supplier[] suppliers = mapper.Map<Supplier[]>(supliersDTOs);

        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        return $"Successfully imported {suppliers.Length}";
    }
}