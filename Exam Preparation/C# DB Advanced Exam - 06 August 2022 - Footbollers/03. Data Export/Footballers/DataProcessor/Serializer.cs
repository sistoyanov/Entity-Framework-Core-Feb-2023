namespace Footballers.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.DataProcessor.ImportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            IMapper mapper = CreateMapper();

            ExportCoachDTO[] coaches = context.Coaches
            .AsNoTracking()
            .Where(c => c.Footballers.Count() > 0)
            .ProjectTo<ExportCoachDTO>(mapper.ConfigurationProvider)
            .OrderByDescending(c => c.FootballersCount)
            .ThenBy(c => c.CoachName)
            .ToArray();

            return XmlHelper.Serialize(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {


            var teams = context.Teams
            .ToArray()
            .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
            .Select(t => new 
            {
                Name = t.Name,
                Footballers = t.TeamsFootballers
                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                    .ThenBy(tf => tf.Footballer.Name)
                    .Select(tf => new 
                    {
                        FootballerName = tf.Footballer.Name,
                        ContractStartDate = tf.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                        PositionType = tf.Footballer.PositionType.ToString()
                    })
                    .ToArray()
            })
            .OrderByDescending(t => t.Footballers.Count())
            .ThenBy(t => t.Name)
            .Take(5)
            .ToArray();

            var output = JsonConvert.SerializeObject(teams, Formatting.Indented);

            return output;
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<FootballersProfile>());

            return config.CreateMapper();
        }
    }


}
