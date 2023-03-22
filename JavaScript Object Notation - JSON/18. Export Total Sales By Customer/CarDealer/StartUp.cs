using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System.IO;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        string result = GetTotalSalesByCustomer(context);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetTotalSalesByCustomer(CarDealerContext context)
    {

        var salesByCustomer = context.Customers
            .AsNoTracking()
            .Where(c => c.Sales.Count() > 0)
            .Select(c => new
            {
                fullName = c.Name,
                boughtCars = c.Sales.Count(),
                spentMoney = c.Sales.SelectMany(s => s.Car.PartsCars.Select(cp => cp.Part.Price)).Sum()
            })
            .OrderByDescending(c => c.spentMoney)
            .ThenByDescending(c => c.boughtCars)
            .ToArray();

        return JsonConvert.SerializeObject(salesByCustomer, Formatting.Indented);
    }
}