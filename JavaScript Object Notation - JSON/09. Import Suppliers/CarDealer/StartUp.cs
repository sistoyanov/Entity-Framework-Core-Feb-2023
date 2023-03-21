using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
        string inputJson = File.ReadAllText(@"../../../Datasets/suppliers.json");

        string result = ImportSuppliers(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportSuppliers(CarDealerContext context, string inputJson)
    {
        ImportSupplierDTO[] supplierDTOs = JsonConvert.DeserializeObject<ImportSupplierDTO[]>(inputJson)!;
        IMapper mapper = MapperProvider();
        Supplier[] suppliers = mapper.Map<Supplier[]>(supplierDTOs);

        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        return $"Successfully imported {suppliers.Length}.";
    }
}