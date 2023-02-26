namespace SoftUni;

using System.Linq;
using System.Text;
using Data;
using SoftUni.Models;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(RemoveTown(dbContext));
    }

    public static string RemoveTown(SoftUniContext context)
    {
        Town townToRemove = context.Towns.Where(t => t.Name == "Seattle").FirstOrDefault()!;

        List<Address> addressesToRemove = context.Addresses
              .Where(a => a.TownId == townToRemove.TownId)
              .ToList();

        int[] addressesToRemoveIds = addressesToRemove.Select(a => a.AddressId).ToArray();

        var employees = context.Employees
            .Where(e => addressesToRemoveIds.Contains((int)e.AddressId!))
            .ToArray();

        foreach (var employee in employees)
        {
            employee.AddressId = null;
        }


        context.Addresses.RemoveRange(addressesToRemove);

        context.Towns.Remove(townToRemove);

        context.SaveChanges();

        return $"{addressesToRemove.Count()} addresses in Seattle were deleted";
    }
}


