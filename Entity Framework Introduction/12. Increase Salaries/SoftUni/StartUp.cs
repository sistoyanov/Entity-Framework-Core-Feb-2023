namespace SoftUni;

using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(IncreaseSalaries(dbContext));
    }

    public static string IncreaseSalaries(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employees = context.Employees
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Where(e => e.Department.Name == "Engineering" || 
                        e.Department.Name == "Tool Design" || 
                        e.Department.Name == "Marketing" || 
                        e.Department.Name == "Information Services")
            .ToArray();

        foreach (var employee in employees)
        {
            employee.Salary *= 1.12m;
        }

        context.SaveChanges();

        foreach ( var e in employees) 
        {
            output.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
        }

        return output.ToString().TrimEnd();
    }
}


