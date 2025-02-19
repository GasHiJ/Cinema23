using AutoMapper;
using Cinema.Core.Models;
using Cinema.BLL.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cinema.BLL.Mapping{ 
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
        CreateMap<Movie, MovieDto>().ReverseMap();
        CreateMap<Session, SessionDto>()
            .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title))
            .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.Hall.Name))
            .ReverseMap();
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Session.Movie.Title))
            .ForMember(dest => dest.SessionStartTime, opt => opt.MapFrom(src => src.Session.StartTime))
            .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.Session.Hall.Name))
            .ReverseMap();
        }
    }
}
