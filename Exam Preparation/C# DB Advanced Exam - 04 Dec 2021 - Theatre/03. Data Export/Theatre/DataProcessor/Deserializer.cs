namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            TimeSpan oneHour = TimeSpan.FromHours(1);

            ImportPlayDTO[] ImportPlayDTOs = XmlHelper.Deserialize<ImportPlayDTO[]>(xmlString, "Plays");

            List<Play> plays = new List<Play>();

            foreach (ImportPlayDTO playDTO in ImportPlayDTOs)
            {
                TimeSpan currentDuration = TimeSpan.ParseExact(playDTO.Duration, "c", CultureInfo.InvariantCulture);

                if (!IsValid(playDTO) || currentDuration < oneHour || !Enum.IsDefined(typeof(Genre), playDTO.Genre))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = playDTO.Title, 
                    Duration = currentDuration,
                    Rating = playDTO.Rating,
                    Genre = (Genre)Enum.Parse(typeof(Genre), playDTO.Genre),
                    Description = playDTO.Description,
                    Screenwriter = playDTO.Screenwriter
                };

                plays.Add(play);

                output.AppendLine(String.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();

            ImportCastDTO[] ImportCastDTOs = XmlHelper.Deserialize<ImportCastDTO[]>(xmlString, "Casts");

            List<Cast> casts = new List<Cast>();

            foreach (ImportCastDTO castDTO in ImportCastDTOs)
            {

                if (!IsValid(castDTO))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = castDTO.FullName,
                    IsMainCharacter = castDTO.IsMainCharacter,
                    PhoneNumber = castDTO.PhoneNumber,
                    PlayId = castDTO.PlayId
                };

                casts.Add(cast);

                output.AppendLine(String.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();

            ImportTheatreDTO[] ImportTheatreDTOs = JsonConvert.DeserializeObject<ImportTheatreDTO[]>(jsonString)!; //jsonSettings

            List<Theatre> theatres = new List<Theatre>();

            foreach (ImportTheatreDTO theatreDTO in ImportTheatreDTOs)
            {
                if (!IsValid(theatreDTO))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = theatreDTO.Name,
                    NumberOfHalls = theatreDTO.NumberOfHalls,
                    Director= theatreDTO.Director,
                };

                foreach (ImportTicketDTO ticketDTO in theatreDTO.Tickets)
                {
                    if (!IsValid(ticketDTO))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    theatre.Tickets.Add(new Ticket() 
                    {
                        Price = ticketDTO.Price,
                        RowNumber = ticketDTO.RowNumber,
                        PlayId = ticketDTO.PlayId
                    });
                }

                theatres.Add(theatre);
                output.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
