namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            //using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            BookShopContext context = new BookShopContext();

            string input = Console.ReadLine();
            Console.WriteLine(GetBooksByAgeRestriction(context, input));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder output = new StringBuilder();

            Enum.TryParse<AgeRestriction>(command, true, out AgeRestriction ageRestriction);

            var booksTitles = context.Books
                               .ToArray()
                               .Where(b => b.AgeRestriction == ageRestriction)
                               .Select(b => new
                               {
                                   b.Title
                               })
                               .OrderBy(b => b.Title)
                               .ToArray();

            foreach (var book in booksTitles)
            {
                output.AppendLine(book.Title);
            }

            return output.ToString().TrimEnd();
        }
    }
}





















