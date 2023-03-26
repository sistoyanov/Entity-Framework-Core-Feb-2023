namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportCoachDTO[] ImportCoachDTOs = XmlHelper.Deserialize<ImportCoachDTO[]>(xmlString, "Coaches");

            List<Coach> coaches = new List<Coach>();

            foreach (ImportCoachDTO coachDTO in ImportCoachDTOs)
            {
                if (!IsValid(coachDTO) || string.IsNullOrEmpty(coachDTO.Nationality))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = coachDTO.Name,
                    Nationality = coachDTO.Nationality
                };


                foreach (ImportFootballerDTO footballerDTO in coachDTO.Footballers)
                {
                    if (!IsValid(footballerDTO) || DateTime.ParseExact(footballerDTO.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) > 
                                                     DateTime.ParseExact(footballerDTO.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDTO.Name,
                        ContractStartDate = DateTime.ParseExact(footballerDTO.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ContractEndDate = DateTime.ParseExact(footballerDTO.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BestSkillType = footballerDTO.BestSkillType,
                        PositionType = footballerDTO.PositionType,
                    };

                    coach.Footballers.Add(footballer);
                }

                coaches.Add(coach);

                output.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportTeamDTO[] teamDTOs = JsonConvert.DeserializeObject<ImportTeamDTO[]>(jsonString)!;

            List<Team> teams = new List<Team>();

            foreach (ImportTeamDTO teamDTO in teamDTOs)
            {
                if (!IsValid(teamDTO) || teamDTO.Trophies == 0)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = teamDTO.Name,
                    Nationality = teamDTO.Nationality,
                    Trophies = teamDTO.Trophies
                };

                foreach (int footballerId in teamDTO.Footballers.Distinct())
                {
                    Footballer footballer = context.Footballers.FirstOrDefault(f => f.Id == footballerId)!;

                    if (footballer == null)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer teamFootballer = new TeamFootballer() { Footballer = footballer };
                    team.TeamsFootballers.Add(teamFootballer);
                }

                teams.Add(team);
                output.AppendLine(String.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            context.Teams.AddRange(teams);
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
}
