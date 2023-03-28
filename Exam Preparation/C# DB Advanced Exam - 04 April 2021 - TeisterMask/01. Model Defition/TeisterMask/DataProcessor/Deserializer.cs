// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor;

using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

using Data;
using System.Text;
using TeisterMask.DataProcessor.ImportDto;

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
        ImportProjectDTO[] ImportShellsDTOs = XmlHelper.Deserialize<ImportProjectDTO[]>(xmlString, "Projects");

        List<Shell> projects = new List<Shell>();

        foreach (ImportShellsDTO shellsDTO in ImportShellsDTOs)
        {
            if (!IsValid(shellsDTO))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            Shell shell = new Shell()
            {
                ShellWeight = shellsDTO.ShellWeight,
                Caliber = shellsDTO.Caliber
            };

            projects.Add(shell);

            output.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
        }

        context.Shells.AddRange(projects);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    public static string ImportEmployees(TeisterMaskContext context, string jsonString)
    {
        throw new NotImplementedException();
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}