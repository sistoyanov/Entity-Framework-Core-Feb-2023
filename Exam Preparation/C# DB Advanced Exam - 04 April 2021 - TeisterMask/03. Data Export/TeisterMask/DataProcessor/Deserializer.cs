// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor;

using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

using Data;
using System.Text;
using TeisterMask.DataProcessor.ImportDto;
using ProductShop.Utilities;
using TeisterMask.Data.Models;
using System.Globalization;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Net.Sockets;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedProject
        = "Successfully imported project - {0} with {1} tasks.";

    private const string SuccessfullyImportedEmployee
        = "Successfully imported employee - {0} with {1} tasks.";

    public static string ImportProjects(TeisterMaskContext context, string xmlString)
    {
        StringBuilder output = new StringBuilder();
        ImportProjectDTO[] ImportProjectDTOs = XmlHelper.Deserialize<ImportProjectDTO[]>(xmlString, "Projects");

        List<Project> projects = new List<Project>();

        foreach (ImportProjectDTO projectDTO in ImportProjectDTOs)
        {
            if (!IsValid(projectDTO))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            //DateTime openDate;
            DateTime? dueDate = null;

            //bool isOpenDateValid = DateTime.TryParseExact(projectDTO.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out openDate);

            //if (!isOpenDateValid)
            //{
            //    output.AppendLine(ErrorMessage);
            //    continue;
            //}

            if (!String.IsNullOrEmpty(projectDTO.DueDate))
            {
                bool isDueDateValid = DateTime.TryParseExact(projectDTO.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDueDate);

                if (!isDueDateValid)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                dueDate = validDueDate;
            }

            Project project = new Project()
            {
               Name = projectDTO.Name,
               OpenDate = DateTime.ParseExact(projectDTO.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
               DueDate = dueDate
            };

            foreach (ImportTaskDTO taskDTO in projectDTO.Tasks)
            {

                if (!IsValid(taskDTO) || DateTime.ParseExact(taskDTO.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < project.OpenDate 
                                      || DateTime.ParseExact(taskDTO.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) > project.DueDate)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Task task = new Task()
                {
                    Name = taskDTO.Name,
                    OpenDate = DateTime.ParseExact(taskDTO.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DueDate = DateTime.ParseExact(taskDTO.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ExecutionType = taskDTO.ExecutionType,
                    LabelType = taskDTO.LabelType
                };

                project.Tasks.Add(task);
            }

            projects.Add(project);

            output.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
        }

        context.Projects.AddRange(projects);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    public static string ImportEmployees(TeisterMaskContext context, string jsonString)
    {
        StringBuilder output = new StringBuilder();

        ImportEmployeeDTO[] ImportEmployeeDTOs = JsonConvert.DeserializeObject<ImportEmployeeDTO[]>(jsonString)!; //jsonSettings

        List<Employee> employees = new List<Employee>();

        foreach (ImportEmployeeDTO employeeDTO in ImportEmployeeDTOs)
        {
            if (!IsValid(employeeDTO))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            Employee employee = new Employee()
            {
                Username = employeeDTO.Username,
                Email = employeeDTO.Email,
                Phone = employeeDTO.Phone
            };

            foreach (int taskId in employeeDTO.Tasks.Distinct())
            {
                //Task task = context.Tasks.FirstOrDefault(t => t.Id == taskId)!;

                if (!context.Tasks.Any(t => t.Id == taskId))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                employee.EmployeesTasks.Add(new EmployeeTask() { TaskId = taskId});
            }

            employees.Add(employee);
            output.AppendLine(String.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
        }

        context.Employees.AddRange(employees);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}