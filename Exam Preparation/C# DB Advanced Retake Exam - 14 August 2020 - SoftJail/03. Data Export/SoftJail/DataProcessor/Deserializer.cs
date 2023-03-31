namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();

            ImportDepartmentDTO[] ImportDepartmentDTOs = JsonConvert.DeserializeObject<ImportDepartmentDTO[]>(jsonString)!;
            List<Department> departments= new List<Department>();
            

            foreach (ImportDepartmentDTO departmentDTO in ImportDepartmentDTOs)
            {
                if (!IsValid(departmentDTO) || departmentDTO.Cells.Length == 0)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                List<Cell> cells = new List<Cell>();
                bool hasInvalidCell = false;

                foreach (ImportCellDTO cellDTO in departmentDTO.Cells) 
                {
                    if (!IsValid(cellDTO))
                    {
                        output.AppendLine(ErrorMessage);
                        hasInvalidCell= true;
                        break;
                    }

                    Cell cell = new Cell()
                    {
                        CellNumber= cellDTO.CellNumber,
                        HasWindow = cellDTO.HasWindow
                    };

                    cells.Add(cell);
                }

                if (hasInvalidCell)
                {
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentDTO.Name,
                    Cells = cells
                };

                departments.Add(department);
                output.AppendLine(String.Format(SuccessfullyImportedDepartment, department.Name, department.Cells.Count));
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();

            ImportPrisonerDTO[] ImportPrisonerDTOs = JsonConvert.DeserializeObject<ImportPrisonerDTO[]>(jsonString)!;
            List<Prisoner> prisoners = new List<Prisoner>();


            foreach (ImportPrisonerDTO prisonerDTO in ImportPrisonerDTOs)
            {
                if (!IsValid(prisonerDTO))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                List<Mail> mails = new List<Mail>();
                bool hasInvalidMail = false;

                foreach (ImportMailDTO mailDTO in prisonerDTO.Mails)
                {
                    if (!IsValid(mailDTO))
                    {
                        output.AppendLine(ErrorMessage);
                        hasInvalidMail = true;
                        break;
                    }

                    Mail mail = new Mail()
                    {
                       Description = mailDTO.Description,
                       Sender = mailDTO.Sender,
                       Address = mailDTO.Address
                    };

                    mails.Add(mail);
                }

                if (hasInvalidMail)
                {
                    continue;
                }

                DateTime.TryParse(prisonerDTO.ReleaseDate, out DateTime releaseDate);

                Prisoner prisoner = new Prisoner()
                {
                    FullName= prisonerDTO.FullName,
                    Nickname= prisonerDTO.Nickname,
                    Age= prisonerDTO.Age,
                    IncarcerationDate = DateTime.ParseExact(prisonerDTO.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = releaseDate,
                    Bail = prisonerDTO.Bail,
                    CellId = prisonerDTO.CellId,
                    Mails = mails
                };

                prisoners.Add(prisoner);
                output.AppendLine(String.Format(SuccessfullyImportedPrisoner, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportOfficerDTO[] ImportOfficerDTOs = XmlHelper.Deserialize<ImportOfficerDTO[]>(xmlString, "Officers");

            List<Officer> officers = new List<Officer>();

            foreach (ImportOfficerDTO officerDTO in ImportOfficerDTOs)
            {
                if (!IsValid(officerDTO) || !Enum.IsDefined(typeof(Position), officerDTO.Position) || !Enum.IsDefined(typeof(Weapon), officerDTO.Weapon))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Officer officer = new Officer() 
                { 
                    FullName = officerDTO.FullName,
                    Salary = officerDTO.Salary,
                    Position = (Position)Enum.Parse(typeof(Position), officerDTO.Position),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), officerDTO.Weapon),
                    DepartmentId = officerDTO.DepartmentId
                };

                foreach (ImportOfficerPrisonerDTO officerPrisonerDTO in officerDTO.Prisoners)
                {
                    OfficerPrisoner officerPrisoner = new OfficerPrisoner() { PrisonerId = officerPrisonerDTO.PrisonerId };
                    officer.OfficerPrisoners.Add(officerPrisoner);
                }

                officers.Add(officer);

                output.AppendLine(String.Format(SuccessfullyImportedOfficer, officer.FullName, officer.OfficerPrisoners.Count));
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}