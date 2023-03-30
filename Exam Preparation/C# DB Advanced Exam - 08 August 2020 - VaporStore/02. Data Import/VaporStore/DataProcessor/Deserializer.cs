namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();

            ImportGameDTO[] importGameDTOs = JsonConvert.DeserializeObject<ImportGameDTO[]>(jsonString)!;

            List<Game> games = new List<Game>();
            List<Developer> developers = new List<Developer>();
            List<Genre> genres = new List<Genre>();
            List<Tag> tags = new List<Tag>();

            foreach (ImportGameDTO gameDTO in importGameDTOs)
            {
                if (!IsValid(gameDTO) || gameDTO.Tags.Length == 0)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Game game = new Game()
                {
                    Name= gameDTO.Name,
                    Price= gameDTO.Price,
                    ReleaseDate = DateTime.ParseExact(gameDTO.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                };


                Developer developer = developers.FirstOrDefault(d => d.Name == gameDTO.Developer)!;

                if (developer == null)
                {
                    Developer newDeveloper = new Developer(){ Name = gameDTO.Developer };

                    developers.Add(newDeveloper);
                    game.Developer = newDeveloper;
                }
                else
                {
                    game.Developer = developer;
                }


                Genre genre = genres.FirstOrDefault(g => g.Name == gameDTO.Genre)!;

                if (genre == null)
                {
                    Genre newGenre = new Genre() { Name = gameDTO.Genre };

                    genres.Add(newGenre);
                    game.Genre = newGenre;
                }
                else
                {
                    game.Genre = genre;
                }


                foreach (string tag in gameDTO.Tags)
                {
                    Tag importTag = tags.FirstOrDefault(t => t.Name == tag)!;

                    if (importTag == null)
                    {
                        Tag newTag = new Tag() { Name = tag };
                        tags.Add(newTag);

                        game.GameTags.Add(new GameTag() 
                        { 
                            Game = game, 
                            Tag = newTag 
                        });
                    }
                    else
                    {
                        tags.Add(importTag);

                        game.GameTags.Add(new GameTag() 
                        {
                            Game = game, 
                            Tag = importTag 
                        });
                    }

                }


                games.Add(game);
                output.AppendLine(String.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();

            ImportUserDTO[] importUserDTOs = JsonConvert.DeserializeObject<ImportUserDTO[]>(jsonString)!;

            List<User> users = new List<User>();

            foreach (ImportUserDTO userDTO in importUserDTOs)
            {
                if (!IsValid(userDTO) || userDTO.Cards.Length == 0)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                List<Card> cards = new List<Card>();

                foreach (ImportCardDTO cardDTO in userDTO.Cards)
                {
                    if (!IsValid(cardDTO)) //|| Enum.IsDefined(typeof(CardType), cardDTO.Type)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    Card card = new Card() 
                    {
                        Number= cardDTO.Number,
                        Cvc = cardDTO.Cvc,
                        Type =  cardDTO.Type //(CardType)Enum.Parse(typeof(CardType),
                    };

                    cards.Add(card);
                }

                User user = new User()
                {
                    FullName = userDTO.FullName,
                    Username = userDTO.Username,
                    Email = userDTO.Email,
                    Age = userDTO.Age,
                    Cards = cards
                };

                users.Add(user);
                output.AppendLine(String.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportPurchaseDTO[] ImportPurchaseDTOs = XmlHelper.Deserialize<ImportPurchaseDTO[]>(xmlString, "Purchases");

            List<Purchase> purchases = new List<Purchase>();

            foreach (ImportPurchaseDTO purchaseDTO in ImportPurchaseDTOs)
            {
                if (!IsValid(purchaseDTO))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Game game = context.Games.FirstOrDefault(g => g.Name == purchaseDTO.Title)!;

                if (game == null)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Card card = context.Cards.FirstOrDefault(c => c.Number == purchaseDTO.Card)!;

                if (card == null) 
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Purchase purchase = new Purchase()
                {
                    Type = purchaseDTO.Type,
                    ProductKey = purchaseDTO.Key,
                    Date = DateTime.ParseExact(purchaseDTO.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Card = card,
                    Game = game
                };


                purchases.Add(purchase);
                User user = context.Users.FirstOrDefault(u => u.Cards.Any(c => c.Number == purchaseDTO.Card))!;

                output.AppendLine(String.Format(SuccessfullyImportedPurchase, game.Name, user.Username));
            }

            context.Purchases.AddRange(purchases);
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