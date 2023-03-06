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

            Console.WriteLine(GetBooksByPrice(context));
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();

            var booksTitles = context.Books
                               .AsNoTracking()
                               .Where(b => b.Price > 40)
                               .OrderByDescending(b => b.Price)
                               .Select(b => new
                               {
                                   b.Title,
                                   b.Price
                               })
                               .ToArray();

            foreach ( var book in booksTitles ) 
            { 
                output.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return output.ToString().TrimEnd();
        }
    }
}





















