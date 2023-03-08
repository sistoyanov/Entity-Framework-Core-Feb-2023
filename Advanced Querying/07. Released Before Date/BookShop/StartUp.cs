namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            //using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            BookShopContext context = new BookShopContext();

            Console.WriteLine("Enter date: ");
            string date = Console.ReadLine();

            Console.WriteLine(GetBooksReleasedBefore(context, date));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder output = new StringBuilder();
            DateTime stringToDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                               .AsNoTracking()
                               .Where(b => b.ReleaseDate < stringToDate)
                               .OrderByDescending(b => b.ReleaseDate)
                               .Select(b => new
                               {
                                   b.Title,
                                   b.EditionType,
                                   b.Price
                               })
                               .ToArray();

            foreach ( var book in books ) 
            {
                output.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return output.ToString().TrimEnd();
        }
    }
}





















