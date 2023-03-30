namespace VaporStore.DataProcessor
{ 
    using Data;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.Globalization;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ExportDto;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .ToArray()
                .Select(g => new 
                { 
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
                                .Where(ga => ga.Purchases.Any())
                                .ToArray()
                                .Select(ga => new 
                                {
                                    Id = ga.Id,
                                    Title = ga.Name,
                                    Developer = ga.Developer.Name,
                                    Tags = String.Join(", ", ga.GameTags.Select(t => t.Tag.Name)),
                                    Players = ga.Purchases.Count
                                })
                                .OrderByDescending(ga => ga.Players)
                                .ThenBy(ga => ga.Id)
                                .ToArray(),
                    TotalPlayers = g.Games.Sum(g => g.Purchases.Count)
                })
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToArray();
            
            var output = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return output;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            var users = context.Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == purchaseType)))
                .Select(u => new ExportUserDTO
                { 
                    UserName = u.Username,
                    Purchases = context.Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type.ToString() == purchaseType)
                        .OrderBy(p => p.Date)
                        .Select(p => new ExportPurchaseDTO 
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameDTO 
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .ToArray(),
                    TotalSpent = context.Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type.ToString() == purchaseType)
                        .Sum(p => p.Game.Price)
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.UserName)
                .ToArray();
            
            return XmlHelper.Serialize(users, "Users");
        }
    }
}