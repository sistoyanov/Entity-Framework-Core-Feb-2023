namespace BookShop
{
    using BookShop.Models;
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

            Console.WriteLine(GetMostRecentBooks(context));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();

            var categories = context.Categories
                               .AsNoTracking()
                               .OrderBy(c => c.Name)
                               .Select(c => new
                               {
                                   c.Name,
                                   Books = c.CategoryBooks
                                   .OrderByDescending(cb => cb.Book.ReleaseDate)
                                   .Take(3)
                                   .Select(cb => new 
                                   {
                                       BookTitle = cb.Book.Title,
                                       BookReleaseYear = cb.Book.ReleaseDate.Value.Year
                                   })
                                   .ToArray()

                               })
                               .ToArray();

            foreach (var categorie in categories)
            {
                output.AppendLine($"--{categorie.Name}");

                foreach (var book in categorie.Books)
                {
                    output.AppendLine($"{book.BookTitle} ({book.BookReleaseYear})");
                }
            }

            return output.ToString().TrimEnd();
        }
    }
}





















