using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Globalization;

namespace CarDealer;

public class CarDealerProfile : Profile
{
    public CarDealerProfile()
    {
        //Supplier
        this.CreateMap<ImportSupplierDTO, Supplier>();

        //Part
        this.CreateMap<ImportPartDTO, Part>();

        //Car
        this.CreateMap<ImportCarDTO, Car>();

        this.CreateMap<Car, ExportCarDTO>();

        //Customer
        this.CreateMap<importCustomerDTO, Customer>();

        this.CreateMap<Customer, ExportCustomerDTO>()
            .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => s.BirthDate.ToString("dd/MM/yyyy")));

        //Sale
        this.CreateMap<ImportSaleDTO, Sale>();
          
    }
}
