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

            IncreasePrices(context);
        }

        public static void IncreasePrices(BookShopContext context)
        {

            Book[] books = context.Books.Where(b => b.ReleaseDate!.Value.Year < 2010).ToArray();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }
    }
}





















