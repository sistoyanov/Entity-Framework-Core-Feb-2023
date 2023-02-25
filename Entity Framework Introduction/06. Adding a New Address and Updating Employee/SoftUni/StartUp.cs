namespace SoftUni;

using System.Text;
using Data;
using Models;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(AddNewAddressToEmployee(dbContext));
    }

    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAdress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
            
        };

        Employee? searchedEmpoloyee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
        searchedEmpoloyee!.Address = newAdress;

        context.SaveChanges();

        StringBuilder output = new StringBuilder();

        var employess = context.Employees
               .OrderByDescending(e => e.AddressId)
               .Take(10)
               .Select(e => new
               {
                   Address = e.Address!.AddressText
               })
               .ToArray();

        foreach ( var e in employess ) 
        { 
          output.AppendLine($"{e.Address}");
        }

        return output.ToString().TrimEnd();
    }
}


