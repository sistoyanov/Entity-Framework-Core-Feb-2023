namespace Trucks.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            IMapper mapper = CreateMapper();

            ExportDespatcherDTO[] despatchers = context.Despatchers
            .AsNoTracking()
            .Where(d => d.Trucks.Count() > 0)
            .ProjectTo<ExportDespatcherDTO>(mapper.ConfigurationProvider)
            .OrderByDescending(d => d.TrucksCount)
            .ThenBy(d => d.Name)
            .ToArray();

            return XmlHelper.Serialize(despatchers, "Despatchers");
        }

        //[JsonConverter(typeof(StringEnumConverter))]
        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
            //.AsNoTracking()
            .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
            .ToArray()
            .Select(c => new
            {
                c.Name,
                Trucks = c.ClientsTrucks
                         .Where(ct => ct.Truck.TankCapacity >= capacity)
                         //.ToArray()
                         .OrderBy(ct => ct.Truck.MakeType.ToString())//.ToString())
                         .ThenByDescending(ct => ct.Truck.CargoCapacity)
                         .Select(ct => new
                         {
                             TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                             VinNumber = ct.Truck.VinNumber,
                             TankCapacity = ct.Truck.TankCapacity,
                             CargoCapacity = ct.Truck.CargoCapacity,
                             CategoryType = ct.Truck.CategoryType.ToString(),
                             MakeType = ct.Truck.MakeType.ToString(),
                         })
                         .ToArray()
            })
            .OrderByDescending(c => c.Trucks.Count())
            .ThenBy(c => c.Name)
            .Take(10)
            .ToArray();

            return JsonConvert.SerializeObject(clients, Formatting.Indented);
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TrucksProfile>());

            return config.CreateMapper();
        }
    }
}
