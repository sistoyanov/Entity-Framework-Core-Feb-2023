
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using ProductShop.Utilities;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .ToArray()
                .Select(s => new 
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                        .Where(g => g.GunType.ToString() ==  "AntiAircraftGun") //(GunType)Enum.Parse(typeof(GunType), "AntiAircraftGun")
                        .ToArray()
                        .Select(g => new 
                        {
                            GunType = g.GunType.ToString(),
                            GunWeight = g.GunWeight,
                            BarrelLength = g.BarrelLength,
                            Range = g.Range > 3000 ? "Long-range" : "Regular range"
                        })
                        .OrderByDescending(g => g.GunWeight)
                        .ToArray()

                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            var output = JsonConvert.SerializeObject(shells, Formatting.Indented);

            return output;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var guns = context.Guns
                .AsNoTracking()
                .Where(g => g.Manufacturer.ManufacturerName.ToLower() == manufacturer.ToLower())
                //.ToArray()
                .Select(g => new ExportGunDTO 
                { 
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight.ToString(),
                    BarrelLength = g.BarrelLength.ToString(),
                    Range = g.Range.ToString(),
                    Countries = g.CountriesGuns
                        .Where(cg => cg.Country.ArmySize > 4_500_000)
                        .Select(cg => new ExportCountryDTO 
                        {
                            Country = cg.Country.CountryName,
                            ArmySize = cg.Country.ArmySize.ToString()
                        })
                        .OrderBy(cg => cg.ArmySize)
                        .ToArray()
                })
                .OrderBy(g => g.BarrelLength)
                .ToArray();
            
            return XmlHelper.Serialize(guns, "Guns");
        }
    }
}
