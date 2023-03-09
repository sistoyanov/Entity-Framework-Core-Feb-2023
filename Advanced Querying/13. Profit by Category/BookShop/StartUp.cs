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

            Console.WriteLine(GetTotalProfitByCategory(context));
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();

            var categories = context.Categories
                               .AsNoTracking()
                               .Select(c => new
                               {
                                   CategoryName = c.Name,
                                   TotalProfit =  c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                               })
                               .OrderByDescending(c => c.TotalProfit)
                               .ThenBy(c => c.CategoryName)
                               .ToArray();

            foreach (var categorie in categories)
            {
                output.AppendLine($"{categorie.CategoryName} ${categorie.TotalProfit:f2}");
            }

            return output.ToString().TrimEnd();
        }
    }
}





















