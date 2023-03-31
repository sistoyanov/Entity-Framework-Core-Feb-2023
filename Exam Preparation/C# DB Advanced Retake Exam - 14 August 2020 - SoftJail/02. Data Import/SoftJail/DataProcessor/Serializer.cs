namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using System.Globalization;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .ToArray()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new 
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                    .ToArray()
                    .Select(po => new 
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(po => po.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(po => po.Officer.Salary), 2)
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var output = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return output;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] validPrisoners = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var prisoners = context.Prisoners
                .ToArray()
                .Where(p => validPrisoners.Contains(p.FullName))
                .Select(p => new ExportPrisonerDTO 
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = DateTime.ParseExact(p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), "yyyy-MM-dd",                    CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails
                    .ToArray()
                    .Select(m => new ExportMessageDTO 
                    {
                        Description = new string (m.Description.Reverse().ToArray())
                    })
                    .ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();


            return XmlHelper.Serialize(prisoners, "Prisoners");
        }
    }
}