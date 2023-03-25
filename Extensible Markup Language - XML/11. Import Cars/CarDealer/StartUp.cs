using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using ProductShop.Utilities;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
        string inputXml = File.ReadAllText(@"../../../Datasets/cars.xml");

        string result = ImportCars(context, inputXml);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

        return config.CreateMapper();
    }

    public static string ImportCars(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();

        ImportCarDTO[] carDTOs = XmlHelper.Deserialize<ImportCarDTO[]>(inputXml, "Cars");

        ICollection<Car> cars = new HashSet<Car>();

        foreach (var car in carDTOs)
        {
            Car newCar = mapper.Map<Car>(car);

            foreach (var part in car.Parts.DistinctBy(p => p.PartId))
            {
                if (part.PartId != null || context.Parts.Any(p => p.Id == part.PartId))
                {
                    PartCar parCar = new PartCar()
                    {
                        PartId = part!.PartId,
                    };

                    newCar.PartsCars.Add(parCar);
                }


            }
            cars.Add(newCar);
        }

        context.Cars.AddRange(cars);
        context.SaveChanges();
    

        return $"Successfully imported {cars.Count}";
    }
}