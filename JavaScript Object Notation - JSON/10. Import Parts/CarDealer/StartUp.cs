using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
        string inputJson = File.ReadAllText(@"../../../Datasets/parts.json");

        string result = ImportParts(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportParts(CarDealerContext context, string inputJson)
    {
        ImportPartDTO[] partDTOs = JsonConvert.DeserializeObject<ImportPartDTO[]>(inputJson)!;
        IMapper mapper = MapperProvider();

        //Part[] parts = partDTOs.Where(p => context.Suppliers.Find(p.SupplierId) != null).Select(p => mapper.Map<Part>(p)).ToArray();
        ICollection<Part> parts = new HashSet<Part>();

        foreach (ImportPartDTO partDTO in partDTOs)
        {
            if (!context.Suppliers.Any(s => s.Id == partDTO.SupplierId))
            {
                continue;
            }

            Part part = mapper.Map<Part>(partDTO);
            parts.Add(part);
        }

        context.Parts.AddRange(parts);
        context.SaveChanges();

        return $"Successfully imported {parts.Count}.";
    }
}