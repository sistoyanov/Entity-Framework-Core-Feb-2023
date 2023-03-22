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

        string result = GetLocalSuppliers(context);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetLocalSuppliers(CarDealerContext context)
    {
        IMapper mapper = MapperProvider();

        ExportSupplierDTO[] suppliers = context.Suppliers
             .AsNoTracking()
             .Where(s => s.IsImporter != true)
             .ProjectTo<ExportSupplierDTO>(mapper.ConfigurationProvider)
             .ToArray();

        //var suppliers = context.Suppliers
        //    .AsNoTracking()
        //    .Where(s => s.IsImporter != true)
        //    .Select(s => new 
        //    {
        //        s.Id, 
        //        s.Name,
        //        PartsCount = s.Parts.Count()
        //    })
        //    .ToArray();

        return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
    }
}