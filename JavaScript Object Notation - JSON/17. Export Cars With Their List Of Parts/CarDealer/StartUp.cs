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

        string result = GetCarsWithTheirListOfParts(context);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {

        var carsWithParts = context.Cars
            .AsNoTracking()
            .Select(c => new
            {
                car = new 
                {
                    c.Make,
                    c.Model,
                    c.TraveledDistance
                },

                parts = c.PartsCars.Select(pc => new 
                {
                    Name = pc.Part.Name,
                    Price = pc.Part.Price.ToString("f2"),
                })
                .ToArray()
                
            })
            .ToArray();

        return JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);
    }
}