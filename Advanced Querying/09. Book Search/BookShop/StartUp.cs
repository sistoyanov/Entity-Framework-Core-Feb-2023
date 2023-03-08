namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            //using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            BookShopContext context = new BookShopContext();

            Console.WriteLine("Enter string: ");
            string input = Console.ReadLine();

            Console.WriteLine(GetBookTitlesContaining(context, input));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {

            var bookTitles = context.Books
                               .AsNoTracking()
                               .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                               .Select(b => b.Title)
                               .OrderBy(b => b)
                               .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }
    }
}





















