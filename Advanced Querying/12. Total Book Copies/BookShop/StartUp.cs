namespace BookShop
{
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

            Console.WriteLine(CountCopiesByAuthor(context));
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();

            var authors = context.Authors
                               .AsNoTracking()
                               .Select(a => new
                               {
                                   a.FirstName,
                                   a.LastName,
                                   BookCopies = a.Books.Sum(b => b.Copies)
                               })
                               .OrderByDescending(a => a.BookCopies)
                               .ToArray();

            foreach (var author in authors)
            {
                output.AppendLine($"{author.FirstName} {author.LastName} - {author.BookCopies}");
            }

            return output.ToString().TrimEnd();
        }
    }
}





















