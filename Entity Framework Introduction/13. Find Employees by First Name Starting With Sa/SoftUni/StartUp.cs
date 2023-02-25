namespace SoftUni;

using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(dbContext));
    }

    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.FirstName.StartsWith("Sa"))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                e.Salary

            })
            .ToArray();

        foreach ( var e in employees) 
        {
            output.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
        }

        return output.ToString().TrimEnd();
    }
}


