namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            //using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            BookShopContext context = new BookShopContext();

            Console.WriteLine(GetGoldenBooks(context));
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var booksTitles = context.Books
                               .AsNoTracking()
                               .Where(b => b.EditionType.Equals(EditionType.Gold) && b.Copies < 5000)
                               .OrderBy(b => b.BookId)
                               .Select(b => b.Title)
                               .ToArray();

            return string.Join(Environment.NewLine, booksTitles);
        }
    }
}





















