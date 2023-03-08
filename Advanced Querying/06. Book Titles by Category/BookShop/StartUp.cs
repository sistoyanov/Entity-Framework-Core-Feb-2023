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

            Console.WriteLine("Enter categories: ");
            string input = Console.ReadLine();

            Console.WriteLine(GetBooksByCategory(context, input));
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                               .AsNoTracking()
                               .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                               .OrderBy(b => b.Title)
                               .Select(b => b.Title)
                               .ToArray();

            return string.Join(Environment.NewLine, books);
        }
    }
}





















