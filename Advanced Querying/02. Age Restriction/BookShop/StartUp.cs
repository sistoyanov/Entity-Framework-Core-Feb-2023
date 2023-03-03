using BookShop.Data;
using System.Text;

namespace _02._Age_Restriction;

public class StartUp
{
    static void Main(string[] args)
    {
       BookShopContext context= new BookShopContext();

        //string input = Console.ReadLine().ToLower();
        Console.WriteLine(GetBooksByAgeRestriction(context, "Minor"));
    }

    public static string GetBooksByAgeRestriction(BookShopContext context, string command)
    {
        StringBuilder output = new StringBuilder();
        
        var booksTitles = context.Books
                           .ToArray()
                           .Where(b => b.AgeRestriction.ToString().ToLower() == command)
                           .Select(b => new
                           {
                               b.Title
                           })
                           .ToArray();

        foreach ( var book in booksTitles ) 
        {
           output.AppendLine(book.Title);
        }

        return output.ToString().TrimEnd();
    }
}