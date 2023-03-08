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

            Console.WriteLine("Enter string: ");
            string input = Console.ReadLine();

            Console.WriteLine(GetBooksByAuthor(context, input));
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder output = new StringBuilder();

            var books = context.Books
                               .AsNoTracking()
                               .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                               .OrderBy(b => b.BookId)
                               .Select(b => new
                               {
                                   b.Title,
                                   AuthorFirstName = b.Author.FirstName,
                                   AuthorLastName = b.Author.LastName
                               })
                               .ToArray();

            foreach (var book in books)
            {
                output.AppendLine($"{book.Title} ({book.AuthorFirstName} {book.AuthorLastName})");
            }

            return output.ToString().TrimEnd();
        }
    }
}





















