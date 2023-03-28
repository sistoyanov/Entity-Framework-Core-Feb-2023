namespace Trucks
{
    using AutoMapper;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ExportDto;

    public class TrucksProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public TrucksProfile()
        {
            this.CreateMap<Truck, ExportTruckDTO>()
                .ForMember(d => d.RegistrationNumber, opt => opt.MapFrom(s => s.RegistrationNumber))
                .ForMember(d => d.Make, opt => opt.MapFrom(s => s.MakeType.ToString()));

            this.CreateMap<Despatcher, ExportDespatcherDTO>()
                .ForMember(d => d.TrucksCount, opt => opt.MapFrom(s => s.Trucks.Count()))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Trucks, opt => opt.MapFrom(s => s.Trucks.OrderBy(t => t.RegistrationNumber).ToArray()));
        }
    }
}
