namespace SoftUni;

using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetAddressesByTown(dbContext));
    }

    public static string GetAddressesByTown(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var addresses = context.Addresses
            .Select(a => new
            {
                EmployeeCount = a.Employees.Count,
                TownName = a.Town!.Name,
                a.AddressText
            })
            .OrderByDescending(a => a.EmployeeCount)
            .ThenBy(a => a.TownName)
            .ThenBy(a => a.AddressText)
            .Take(10)
            .ToArray();

        foreach ( var a in addresses) 
        { 
          output.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");

        }

        return output.ToString().TrimEnd();
    }
}


