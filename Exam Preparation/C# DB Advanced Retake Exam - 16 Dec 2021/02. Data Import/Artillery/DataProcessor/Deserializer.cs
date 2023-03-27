namespace Artillery.DataProcessor;

using Artillery.Data;
using Artillery.Data.Models;
using Artillery.Data.Models.Enums;
using Artillery.DataProcessor.ImportDto;
using Newtonsoft.Json;
using ProductShop.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

public class Deserializer
{
    private const string ErrorMessage =
        "Invalid data.";
    private const string SuccessfulImportCountry =
        "Successfully import {0} with {1} army personnel.";
    private const string SuccessfulImportManufacturer =
        "Successfully import manufacturer {0} founded in {1}.";
    private const string SuccessfulImportShell =
        "Successfully import shell caliber #{0} weight {1} kg.";
    private const string SuccessfulImportGun =
        "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

    public static string ImportCountries(ArtilleryContext context, string xmlString)
    {
        StringBuilder output = new StringBuilder();
        ImputCountryDTO[] ImputCountryDTOs = XmlHelper.Deserialize<ImputCountryDTO[]>(xmlString, "Countries");

        List<Country> countries = new List<Country>();

        foreach (ImputCountryDTO CountryDTO in ImputCountryDTOs)
        {
            if (!IsValid(CountryDTO))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            Country country = new Country()
            {
                CountryName = CountryDTO.CountryName,
                ArmySize = CountryDTO.ArmySize
            };

            countries.Add(country);

            output.AppendLine(String.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
        }

        context.Countries.AddRange(countries);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    public static string ImportManufacturers(ArtilleryContext context, string xmlString)
    {
        StringBuilder output = new StringBuilder();
        ImportManufacturersDTO[] ImportManufacturersDTOs = XmlHelper.Deserialize<ImportManufacturersDTO[]>(xmlString, "Manufacturers");

        List<Manufacturer> manufacturers = new List<Manufacturer>();

        foreach (ImportManufacturersDTO manufacturersDTO in ImportManufacturersDTOs) //.DistinctBy(m => m.ManufacturerName)
        {
            if (!IsValid(manufacturersDTO) || manufacturers.Any(m => m.ManufacturerName == manufacturersDTO.ManufacturerName))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            Manufacturer manufacturer = new Manufacturer()
            {
                ManufacturerName = manufacturersDTO.ManufacturerName,
                Founded = manufacturersDTO.Founded
            };

            manufacturers.Add(manufacturer);

            string[] manufacturerCountry = manufacturer.Founded.Split(", ").ToArray();
            string[] last = manufacturerCountry.Skip(Math.Max(0, manufacturerCountry.Count() - 2)).ToArray();

            output.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, string.Join(", ", last)));
        }

        context.Manufacturers.AddRange(manufacturers);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    public static string ImportShells(ArtilleryContext context, string xmlString)
    {
        StringBuilder output = new StringBuilder();
        ImportShellsDTO[] ImportShellsDTOs = XmlHelper.Deserialize<ImportShellsDTO[]>(xmlString, "Shells");

        List<Shell> shells = new List<Shell>();

        foreach (ImportShellsDTO shellsDTO in ImportShellsDTOs)
        {
            if (!IsValid(shellsDTO))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            Shell shell = new Shell()
            {
                ShellWeight = shellsDTO.ShellWeight,
                Caliber = shellsDTO.Caliber
            };

            shells.Add(shell);

            output.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
        }

        context.Shells.AddRange(shells);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    public static string ImportGuns(ArtilleryContext context, string jsonString)
    {
        StringBuilder output = new StringBuilder();

        //var jsonSettings = new JsonSerializerSettings()
        //{
        //    NullValueHandling = NullValueHandling.Ignore,
        //    Formatting = Formatting.Indented,
        //};

        ImportGunDTO[] ImportGunsDTOs = JsonConvert.DeserializeObject<ImportGunDTO[]>(jsonString)!; //jsonSettings

        List<Gun> guns = new List<Gun>();

        foreach (ImportGunDTO gunDTO in ImportGunsDTOs)
        {
            if (!IsValid(gunDTO) || !Enum.IsDefined(typeof(GunType), gunDTO.GunType))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            Gun gun = new Gun()
            {
                ManufacturerId = gunDTO.ManufacturerId,
                GunWeight = gunDTO.GunWeight,
                BarrelLength = gunDTO.BarrelLength,
                NumberBuild = gunDTO.NumberBuild,
                Range = gunDTO.Range,
                GunType = (GunType)Enum.Parse(typeof(GunType) ,gunDTO.GunType),
                ShellId = gunDTO.ShellId
                //CountriesGuns = gunDTO.Countries.Select(c => new CountryGun()
                //{
                //    CountryId = c.Id,
                //})
                //    .ToArray()
            };

            foreach (ImportCountryIdDTO countryIdDTO in gunDTO.Countries)
            {
                gun.CountriesGuns.Add(new CountryGun
                {
                    CountryId = countryIdDTO.Id,
                    Gun = gun
                });
            }

            guns.Add(gun);
            output.AppendLine(String.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
        }

        context.Guns.AddRange(guns);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }
    private static bool IsValid(object obj)
    {
        var validator = new ValidationContext(obj);
        var validationRes = new List<ValidationResult>();

        var result = Validator.TryValidateObject(obj, validator, validationRes, true);
        return result;
    }
}