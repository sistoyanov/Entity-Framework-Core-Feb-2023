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
        string inputJson = File.ReadAllText(@"../../../Datasets/cars.json");

        string result = ImportCars(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportCars(CarDealerContext context, string inputJson)
    {
        ImportCarDTO[] importCarDTOs = JsonConvert.DeserializeObject<ImportCarDTO[]>(inputJson)!;
        //IMapper mapper = MapperProvider();

        //Car[] cars = mapper.Map<Car[]>(carDTOs);

        //ICollection<Car> cars = new HashSet<Car>();
        //ICollection<PartCar> partsCars = new HashSet<PartCar>();

        //foreach (ImportCarDTO importCar in iportCars)
        //{
        //    Car car = new Car()
        //    {
        //        Make = importCar.Make,
        //        Model = importCar.Model,
        //        TraveledDistance= importCar.TraveledDistance
        //    };

        //    cars.Add(car);

        //    foreach (int partId in importCar.PartsId!)
        //    {

        //        PartCar partCar = new PartCar()
        //        {
        //            CarId = car.Id,
        //            PartId = partId
        //        };

        //        partsCars.Add(partCar);
        //    }

        //}

        foreach (var importCar in importCarDTOs)
        {
            var car = new Car()
            {
                Make = importCar.Make,
                Model = importCar.Model,
                TraveledDistance = importCar.TraveledDistance
            };

            //cars.Add(car);

            context.Cars.Add(car);
            context.SaveChanges();

            foreach (int partId in importCar.PartsId!)
            {
                var part = context.Parts.AsNoTracking().FirstOrDefault(p => p.Id == partId);
                if (part != null)
                {
                    if (!context.Cars.Any(c => c.PartsCars.Any(pc => pc.PartId == partId && pc.CarId == car.Id)))
                    {
                        var partCar = new PartCar
                        {
                            CarId = car.Id,
                            PartId = partId
                        };

                        //partsCars.Add(partCar);
                        context.PartsCars.Add(partCar);
                        context.SaveChanges();
                    }
                }
            }
        }

        //context.Cars.AddRange(cars);
        //context.PartsCars.AddRange(partsCars);
        //context.SaveChanges();

        return $"Successfully imported {importCarDTOs.Length}.";
    }
}