using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        string result = GetSalesWithAppliedDiscount(context);
        Console.WriteLine(result);
    }

    private static IMapper MapperProvider()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
    {

        var salesByCustomer = context.Sales
            .AsNoTracking()
            .Select(s => new 
            {
                car = new 
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TraveledDistance = s.Car.TraveledDistance
                },
                customerName = s.Customer.Name,
                discount = s.Discount.ToString("f2"),
                price = s.Car.PartsCars.Select(pc => pc.Part.Price).Sum().ToString("f2"),
                priceWithDiscount = ((s.Car.PartsCars.Select(pc => pc.Part.Price).Sum()) - (s.Car.PartsCars.Select(pc => pc.Part.Price).Sum() * (s.Discount / 100))).ToString("f2")
            })
            .Take(10)
            .ToArray();

        return JsonConvert.SerializeObject(salesByCustomer, Formatting.Indented);
    }
}