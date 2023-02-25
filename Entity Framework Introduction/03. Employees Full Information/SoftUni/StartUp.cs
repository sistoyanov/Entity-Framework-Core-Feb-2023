namespace SoftUni;
using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetEmployeesFullInformation(dbContext));
    }

    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employess = context.Employees
               .OrderBy(e => e.EmployeeId)
               .Select(e => new
               {
                   e.FirstName,
                   e.LastName,
                   e.MiddleName,
                   e.JobTitle,
                   e.Salary
               })
               .ToArray();

        foreach ( var e in employess ) 
        { 
          output.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return output.ToString().TrimEnd();
    }
}


