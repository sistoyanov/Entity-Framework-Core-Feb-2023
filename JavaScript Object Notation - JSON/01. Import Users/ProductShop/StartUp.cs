using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        string inputJson = File.ReadAllText(@"../../../Datasets/users.json");

        string result = ImportUsers(context, inputJson);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        ImportUserDTO[] userDTOs = JsonConvert.DeserializeObject<ImportUserDTO[]>(inputJson);
        IMapper mapper = MapperProvider();
        ICollection<User> users= new HashSet<User>();

        foreach (ImportUserDTO userDTO in userDTOs)
        {
            User user = mapper.Map<User>(userDTO);
            users.Add(user);
        }

        context.Users.AddRange(users);
        context.SaveChanges();

        return $"Successfully imported {users.Count}"; 
    }
}