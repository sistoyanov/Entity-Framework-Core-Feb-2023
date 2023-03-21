using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
        string inputJson = File.ReadAllText(@"../../../Datasets/sales.json");

        string result = ImportSales(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportSales(CarDealerContext context, string inputJson)
    {
        ImportSaleDTO[] importSaleDTOs = JsonConvert.DeserializeObject<ImportSaleDTO[]>(inputJson)!;
        IMapper mapper = MapperProvider();

        Sale[] sales = mapper.Map<Sale[]>(importSaleDTOs);
        context.Sales.AddRange(sales);
        context.SaveChanges();

        return $"Successfully imported {importSaleDTOs.Length}.";
    }
}