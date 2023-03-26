namespace Footballers
{
    using AutoMapper;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ExportDto;

    // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
    public class FootballersProfile : Profile
    {
        public FootballersProfile()
        {
            this.CreateMap<Footballer, ExportFootballerDTO>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Position, opt => opt.MapFrom(s => s.PositionType.ToString()));

            this.CreateMap<Coach, ExportCoachDTO>()
                .ForMember(d => d.FootballersCount, opt => opt.MapFrom(s => s.Footballers.Count))
                .ForMember(d => d.CoachName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Footballers, opt => opt.MapFrom(s => s.Footballers.OrderBy(f => f.Name).ToArray()));
                
        }
    }
}
