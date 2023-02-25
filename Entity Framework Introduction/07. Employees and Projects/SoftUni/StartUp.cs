namespace SoftUni;

using System.Globalization;
using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetEmployeesInPeriod(dbContext));
    }

    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employess = context.Employees
               //.Where(e => e.EmployeesProjects
               //             .Any(ep => ep.Project!.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
               .Take(10)
               .Select(e => new
               {
                   e.FirstName, 
                   e.LastName, 
                   ManagerFirstName = e.Manager!.FirstName, 
                   ManagerLastName = e.Manager!.LastName,
                   Projects = e.EmployeesProjects
                               .Where(ep => ep.Project!.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                               .Select(ep => new
                               {
                                   ProjectName = ep.Project!.Name,
                                   StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                                   EndDate = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                               })
                               .ToArray()
               })
               .ToArray();

        foreach ( var e in employess ) 
        { 
          output.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

            foreach (var p in e.Projects)
            {
                output.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
            }
        }

        return output.ToString().TrimEnd();
    }
}


