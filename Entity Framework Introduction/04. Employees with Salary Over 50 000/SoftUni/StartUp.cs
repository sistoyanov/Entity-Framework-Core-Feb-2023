namespace SoftUni;

using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetEmployeesWithSalaryOver50000(dbContext));
    }

    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employess = context.Employees
               .Where(e => e.Salary > 50000)
               .OrderBy(e => e.FirstName)
               .Select(e => new
               {
                   e.FirstName,
                   e.Salary
               })
               .ToArray();

        foreach ( var e in employess ) 
        { 
          output.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }

        return output.ToString().TrimEnd();
    }
}


