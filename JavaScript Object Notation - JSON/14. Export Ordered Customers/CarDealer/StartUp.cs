using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        string result = GetOrderedCustomers(context);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetOrderedCustomers(CarDealerContext context)
    {
        IMapper mapper = MapperProvider();

        ExportCustomerDTO[] customerDTOs = context.Customers
             .AsNoTracking()
             .OrderBy(c => c.BirthDate)
             .ThenBy(c => c.IsYoungDriver)
             .ProjectTo<ExportCustomerDTO>(mapper.ConfigurationProvider)
             .ToArray();

        return JsonConvert.SerializeObject(customerDTOs, Formatting.Indented);
    }
}