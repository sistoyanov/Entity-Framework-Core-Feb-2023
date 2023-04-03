namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using ProductShop.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportCreatorDTO[] ImportCreatorDTOs = XmlHelper.Deserialize<ImportCreatorDTO[]>(xmlString, "Creators");

            List<Creator> creators = new List<Creator>();

            foreach (ImportCreatorDTO creatorDTO in ImportCreatorDTOs)
            {
                if (!IsValid(creatorDTO))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator()
                {
                    FirstName = creatorDTO.FirstName,
                    LastName = creatorDTO.LastName,
                };

                foreach (ImportBoardgameDTO boardgameDTO in creatorDTO.Boardgames)
                {
                    if (!IsValid(boardgameDTO))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame()
                    {
                        Name = boardgameDTO.Name,
                        Rating = boardgameDTO.Rating,
                        YearPublished = boardgameDTO.YearPublished,
                        CategoryType = boardgameDTO.CategoryType,
                        Mechanics = boardgameDTO.Mechanics,
                    };

                    creator.Boardgames.Add(boardgame);
                }

                creators.Add(creator);

                output.AppendLine(String.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }

            context.Creators.AddRange(creators);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();

            ImportSellerDTO[] ImportSellerDTOs = JsonConvert.DeserializeObject<ImportSellerDTO[]>(jsonString)!;
            List<Seller> sellers = new List<Seller>();


            foreach (ImportSellerDTO sellerDTO in ImportSellerDTOs)
            {
                if (!IsValid(sellerDTO))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sellerDTO.Name,
                    Address = sellerDTO.Address,
                    Country = sellerDTO.Country,
                    Website = sellerDTO.Website
                };

                foreach (int boardGameId in sellerDTO.Boardgames.Distinct())
                {
                    if (!context.Boardgames.Any(bg => bg.Id == boardGameId))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    seller.BoardgamesSellers.Add(new BoardgameSeller() 
                        { 
                            BoardgameId = boardGameId 
                        });
                }

                sellers.Add(seller);
                output.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }

            context.Sellers.AddRange(sellers);
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
