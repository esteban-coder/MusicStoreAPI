using AutoMapper;
using MusicStore.Dto.Response;
using MusicStore.Entities.Infos;

namespace MusicStore.Services.Profiles;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<SaleInfo, SaleDtoResponse>()
            .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.SaleId))
            //.ForMember(dest => dest.DateEvent, opt => opt.MapFrom(src => src.DateEvent.ToString("dd MMMM yyyy")))
            .ForMember(dest => dest.DateEvent, opt => opt.MapFrom(src => src.DateEvent.ToString("yyyy-MM-dd")))
            //.ForMember(dest => dest.TimeEvent, opt => opt.MapFrom(src => src.DateEvent.ToString("hh:mm tt")))
            .ForMember(dest => dest.TimeEvent, opt => opt.MapFrom(src => src.DateEvent.ToString("HH:mm")))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.OperationNumber, opt => opt.MapFrom(src => src.OperationNumber))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            //.ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate.ToString("dd MMMM yyyy hh:mm tt")))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate.ToString("yyyy-MM-dd HH:mm")))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));

        CreateMap<ReportInfo, ReportDtoResponse>();

    }
}