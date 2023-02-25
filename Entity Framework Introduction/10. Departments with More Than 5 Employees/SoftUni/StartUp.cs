namespace SoftUni;

using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetDepartmentsWithMoreThan5Employees(dbContext));
    }

    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var departments = context.Departments
            .OrderBy(d => d.Employees.Count)
            .ThenBy(d => d.Name)
            .Where(d => d.Employees.Count > 5)
            .Select(d => new
            {
               d.Name,
               ManagerFirstName = d.Manager.FirstName,
               ManagerLastName = d.Manager.LastName,
               Employees = d.Employees
                            .OrderBy(e => e.FirstName)
                            .ThenBy(e => e.LastName)                      
                            .Select(e => new
                            {
                                e.FirstName,
                                e.LastName,
                                e.JobTitle
                            })
                            .ToArray()

            })
            .ToArray();

        foreach ( var d in departments) 
        { 
          output.AppendLine($"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}");

            foreach (var e in d.Employees)
            {
                output.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            }

        }

        return output.ToString().TrimEnd();
    }
}


