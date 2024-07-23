using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;

namespace FilmesAPI.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        CreateMap<Filme, ReadFilmeDto>();
        CreateMap<CreateFilmeDto, Filme>();
        CreateMap<UpdateFilmeDto, Filme>();
        CreateMap<Filme, UpdateFilmeDto>();
        
    }
}
