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
        Console.WriteLine(DeleteProjectById(dbContext));
    }

    public static string DeleteProjectById(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        int[] addressIdToRemove = context.Addresses
              .Where(a => a.TownId == 4)
              .Select(a => a.AddressId)
              .ToArray();

        var employees = context.Employees
            .Where(e => addressIdToRemove.Contains((int)e.AddressId!))
            .ToArray();

        foreach ( var employee in employees )
        {
            employee.AddressId = null;
        }


        context.EmployeesProjects.RemoveRange(employeeProjectsToRemove);

        Project projectsToRemove = context.Projects.Find(2)!;

        context.Projects.Remove(projectsToRemove);

        context.SaveChanges();

        var projects = context.Projects
            .Take(10)
            .ToArray();

        foreach ( var p in projects) 
        {
            output.AppendLine(p.Name);
        }

        return output.ToString().TrimEnd();
    }
}


