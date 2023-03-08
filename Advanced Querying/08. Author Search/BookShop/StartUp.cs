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

            Console.WriteLine(GetAuthorNamesEndingIn(context, input));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder output = new StringBuilder();

            var authors = context.Authors
                               .AsNoTracking()
                               .Where(a => a.FirstName.EndsWith(input))
                               .Select(a => new 
                               { 
                                   FullName = $"{a.FirstName} {a.LastName}" 
                               })
                               .OrderBy(a => a.FullName)
                               .ToArray();

            foreach ( var author in authors )
            {

                output.AppendLine(author.FullName);
            }

            return output.ToString().TrimEnd();
        }
    }
}





















