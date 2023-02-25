namespace SoftUni;

using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetEmployeesFromResearchAndDevelopment(dbContext));
    }

    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employess = context.Employees
               .Where(e => e.Department.Name == "Research and Development")
               .OrderBy(e => e.Salary)
               .ThenByDescending(e => e.FirstName)
               .Select(e => new
               {
                   e.FirstName,
                   e.LastName,
                   Department = e.Department.Name,
                   e.Salary
               })
               .ToArray();

        foreach ( var e in employess ) 
        { 
          output.AppendLine($"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:f2}");
        }

        return output.ToString().TrimEnd();
    }
}


