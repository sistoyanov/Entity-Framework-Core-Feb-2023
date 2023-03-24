using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer;

public class CarDealerProfile : Profile
{
    public CarDealerProfile()
    {
        //Supplier
        this.CreateMap<ImportSupplierDTO, Supplier>();

        //Part
        this.CreateMap<ImportPartDTO, Part>();
    }
}
