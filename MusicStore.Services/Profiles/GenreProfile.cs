using AutoMapper;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;

namespace MusicStore.Services.Profiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDtoResponse>();
        CreateMap<GenreDtoRequest, Genre>();
        // Esto es para hacerlo al reves.ReverseMap();
    }
}