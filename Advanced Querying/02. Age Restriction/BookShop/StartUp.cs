using BookShop.Data;
using BookShop.Models.Enums;
using System.Text;

namespace _02._Age_Restriction;

public class StartUp
{
    static void Main(string[] args)
    {
       BookShopContext context= new BookShopContext();

        string input = Console.ReadLine().ToLower();
        Console.WriteLine(GetBooksByAgeRestriction(context, input));
    }

    public static string GetBooksByAgeRestriction(BookShopContext context, string command)
    {
        StringBuilder output = new StringBuilder();

        Enum.TryParse<AgeRestriction>(command, true ,out AgeRestriction ageRestriction);
      
        var booksTitles = context.Books
                           .ToArray()
                           .Where(b => b.AgeRestriction == ageRestriction)
                           .Select(b => new
                           {
                               b.Title
                           })
                           .OrderBy(b => b.Title)
                           .ToArray();

        foreach ( var book in booksTitles ) 
        {
           output.AppendLine(book.Title);
        }

        return output.ToString().TrimEnd();
    }
}