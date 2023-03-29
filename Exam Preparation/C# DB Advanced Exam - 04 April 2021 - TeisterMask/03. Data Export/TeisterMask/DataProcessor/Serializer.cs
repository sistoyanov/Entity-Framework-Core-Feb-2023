namespace TeisterMask.DataProcessor;

using Data;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using ProductShop.Utilities;
using System.Globalization;
using System.Numerics;
using TeisterMask.Data.Models.Enums;
using TeisterMask.DataProcessor.ExportDto;

public class Serializer
{
    public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
    {
        var projects = context.Projects
            .Where(p => p.Tasks.Any())
            .ToArray()
            .Select(p => new ExportProjectDTO
            {
                TasksCount = p.Tasks.Count,
                ProjectName = p.Name,
                HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                Tasks = p.Tasks
                    .ToArray()
                    .Select(t => new ExportTaskDTO 
                    { 
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
            })
            .OrderByDescending(p => p.TasksCount)
            .ThenBy(p => p.ProjectName)
            .ToArray();

        return XmlHelper.Serialize(projects, "Projects");
    }

    public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
    {
        var employees = context.Employees
            .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
            .ToArray()
            .Select(e => new
            {
                Username = e.Username,
                Tasks = e.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date)
                    .ToArray()
                    .Select( et => new 
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    })
                    .OrderByDescending(et => DateTime.ParseExact(et.DueDate, "d", CultureInfo.InvariantCulture))
                    .ThenBy(et => et.TaskName)
                    .ToArray()
            })
            .OrderByDescending(e => e.Tasks.Count())
            .ThenBy(e => e.Username)
            .Take(10)
            .ToArray();


        var output = JsonConvert.SerializeObject(employees, Formatting.Indented);

        return output;
    }
}