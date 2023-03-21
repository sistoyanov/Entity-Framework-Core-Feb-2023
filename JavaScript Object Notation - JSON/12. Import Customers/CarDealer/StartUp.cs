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
        string inputJson = File.ReadAllText(@"../../../Datasets/customers.json");

        string result = ImportCustomers(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportCustomers(CarDealerContext context, string inputJson)
    {
        ImportCustomerDTO[] importCustomerDTOs = JsonConvert.DeserializeObject<ImportCustomerDTO[]>(inputJson)!;
        IMapper mapper = MapperProvider();

        Customer[] customers = mapper.Map<Customer[]>(importCustomerDTOs);
        context.Customers.AddRange(customers);
        context.SaveChanges();

        return $"Successfully imported {importCustomerDTOs.Length}.";
    }
}