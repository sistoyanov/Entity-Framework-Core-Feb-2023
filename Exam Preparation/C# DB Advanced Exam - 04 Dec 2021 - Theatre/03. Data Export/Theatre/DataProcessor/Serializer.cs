namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                .ToArray()
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                .Select(t => new 
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets.ToArray().Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5).Select(ti => ti.Price).Sum(),
                    Tickets = t.Tickets
                                .ToArray()
                                .Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5)
                                .Select(ti => new 
                                {
                                    Price = ti.Price,
                                    RowNumber = ti.RowNumber
                                })
                                .OrderByDescending(ti => ti.Price)
                                .ToArray()

                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

      

            var output = JsonConvert.SerializeObject(theatres, Formatting.Indented);

            return output;
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            var plays = context.Plays
                .ToArray()
                .Where(p => p.Rating <= raiting)
                .Select(p => new ExportPlayDTO
                {
                    Title = p.Title,
                    Duration = TimeSpan.ParseExact(p.Duration.ToString(), "c", CultureInfo.InvariantCulture).ToString(),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                                .ToArray()
                                .Where(c => c.IsMainCharacter == true)
                                .Select(c => new ExportActorDTO 
                                { 
                                    FullName = c.FullName,
                                    MainCharacter = $"Plays main character in '{p.Title}'."
                                })
                                .OrderByDescending(c => c.FullName)
                                .ToArray(),
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();


            return XmlHelper.Serialize(plays, "Plays");
        }
    }
}
