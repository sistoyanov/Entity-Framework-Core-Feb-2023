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

            Console.WriteLine("Enter lenght: ");
            int lengthCheck = int.Parse(Console.ReadLine());

            Console.WriteLine(CountBooks(context, lengthCheck));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int count = context.Books
                               .AsNoTracking()
                               .Where(b => b.Title.Length > lengthCheck)
                               .Count();

            return count;
        }
    }
}





















