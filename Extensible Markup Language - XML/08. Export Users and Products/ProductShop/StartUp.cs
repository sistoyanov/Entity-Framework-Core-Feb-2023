using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();

        string result = GetUsersWithProducts(context);
        Console.WriteLine(result);
    }

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        using (context)
        {
            var exportedUsers = context.Users
            .AsNoTracking()
            .Include(u => u.ProductsSold)
            .Where(u => u.ProductsSold.Count > 0)
            .OrderByDescending(u => u.ProductsSold.Count)
            .Take(10)
            .ToArray();

            XDocument doc = new XDocument();
            XElement root = new XElement("Users");

            int usersCount = context.Users.Count(u => u.ProductsSold.Count > 0);
            root.Add(new XElement("count", usersCount));

            XElement users = new XElement("users");
            root.Add(users);

            foreach (var usr in exportedUsers)
            {
                var user = new XElement("User");
                int productsSoldCount = usr.ProductsSold.Count();

                var productsSold = new XElement("SoldProducts");
                productsSold.Add( new XElement("count", productsSoldCount));

                var products = new XElement("products");

                foreach (var prd in usr.ProductsSold.OrderByDescending(p => p.Price))
                {
                    var product = new XElement("Product");
                    product.Add(new XElement("name", prd.Name),
                                new XElement("price", prd.Price));

                    products.Add(product);
                }

                productsSold.Add(products);

                user.Add(new XElement("firstName", usr.FirstName),
                          new XElement("lastName", usr.LastName),
                          new XElement("age", usr.Age),
                          productsSold);

                users.Add(user);
            }

            doc.Add(root);
            //doc.Save("employees.xml");

            return doc.ToString();
        }

    }
}