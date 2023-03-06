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

            Console.WriteLine("Enter release year: ");
            int year = int.Parse(Console.ReadLine()!);

            Console.WriteLine(GetBooksNotReleasedIn(context, year));
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksTitles = context.Books
                               .AsNoTracking()
                               .Where(b => b.ReleaseDate!.Value.Year != year)
                               .OrderBy(b => b.BookId)
                               .Select(b => b.Title)
                               .ToArray();

            return string.Join(Environment.NewLine, booksTitles);
        }
    }
}





















