namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creators = context.Creators
                .ToArray()
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorDTO 
                {
                    BoardgamesCount = c.Boardgames.Count,
                    CreatorName = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames
                        .ToArray()
                        .Select(bg => new ExportBoardgameDTO
                        {
                            BoardgameName = bg.Name,
                            BoardgameYearPublished = bg.YearPublished.ToString()
                        })
                        .OrderBy(bg => bg.BoardgameName)
                        .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            return XmlHelper.Serialize(creators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .ToArray()
                .Where(s => s.BoardgamesSellers.Any(bg => bg.Boardgame.YearPublished >= year && bg.Boardgame.Rating <= rating))
                .Select(s => new 
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                        .ToArray()
                        .Where(bg => bg.Boardgame.YearPublished >= year && bg.Boardgame.Rating <= rating)
                        .Select(bg => new 
                        {
                            Name = bg.Boardgame.Name,
                            Rating = bg.Boardgame.Rating,
                            Mechanics = bg.Boardgame.Mechanics,
                            Category = bg.Boardgame.CategoryType.ToString()
                        })
                        .OrderByDescending(bg => bg.Rating)
                        .ThenBy(bg => bg.Name)
                        .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();
            
            var output = JsonConvert.SerializeObject(sellers, Formatting.Indented);

            return output;
        }
    }
}