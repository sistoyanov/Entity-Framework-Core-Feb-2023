using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using ProductShop.Utilities;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
        string inputXml = File.ReadAllText(@"../../../Datasets/parts.xml");

        string result = ImportParts(context, inputXml);
        Console.WriteLine(result);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

        return config.CreateMapper();
    }

    public static string ImportParts(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();

        ImportPartDTO[] partDTOs = XmlHelper.Deserialize<ImportPartDTO[]>(inputXml, "Parts");

        ICollection<Part> parts = new HashSet<Part>();

        foreach (var part in partDTOs)
        {
            if (!context.Suppliers.Any(s => s.Id == part.SupplierId))
            {
                continue;
            }

            Part newPart = mapper.Map<Part>(part);
            parts.Add(newPart);
        }

        context.Parts.AddRange(parts);
        context.SaveChanges();

        return $"Successfully imported {parts.Count}";
    }
}