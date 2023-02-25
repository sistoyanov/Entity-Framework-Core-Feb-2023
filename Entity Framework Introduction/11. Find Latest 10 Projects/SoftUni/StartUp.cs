namespace SoftUni;

using System.Globalization;
using System.Text;
using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        Console.WriteLine(GetLatestProjects(dbContext));
    }

    public static string GetLatestProjects(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var projects = context.Projects
            .OrderBy(p => p.Name)
            .Take(10)
            .Select(p => new
            {
               p.Name,
               p.Description,
               StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)

            })
            .ToArray();

        foreach ( var p in projects) 
        {
            output.AppendLine(p.Name)
                  .AppendLine(p.Description)
                  .AppendLine(p.StartDate);
        }

        return output.ToString().TrimEnd();
    }
}


