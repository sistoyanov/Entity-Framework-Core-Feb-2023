namespace SoftUni;

using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetEmployee147(dbContext));
    }

    public static string GetEmployee147(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employee = context.Employees
            .Where(e => e.EmployeeId == 147)
            .Select(e => new
            {
               e.FirstName, 
               e.LastName,
               e.JobTitle,
               ProjectsNames = e.EmployeesProjects
                                .OrderBy(ep => ep.Project!.Name)
                                .Select(ep => ep.Project!.Name)
                                .ToArray()

            })
            .ToArray();

        foreach ( var e in employee) 
        { 
          output.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");

            foreach (var projectName in e.ProjectsNames)
            {
                output.AppendLine(projectName);
            }

        }

        return output.ToString().TrimEnd();
    }
}


