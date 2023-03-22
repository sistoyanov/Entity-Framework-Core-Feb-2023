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

        string result = GetCarsFromMakeToyota(context);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetCarsFromMakeToyota(CarDealerContext context)
    {
        IMapper mapper = MapperProvider();

        ExportCarDTO[] carDTOs = context.Cars
             .AsNoTracking()
             .Where(c => c.Make == "Toyota")
             .OrderBy(c => c.Model)
             .ThenByDescending(c => c.TraveledDistance)
             .ProjectTo<ExportCarDTO>(mapper.ConfigurationProvider)
             .ToArray();

        return JsonConvert.SerializeObject(carDTOs, Formatting.Indented);
    }
}