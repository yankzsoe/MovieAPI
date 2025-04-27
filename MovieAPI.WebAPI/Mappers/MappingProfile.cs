using AutoMapper;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Features.Movie.Commands.Update;

namespace MovieAPI.WebAPI.Mappers {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CreateUpdateMovieDto, MovieUpdateCommand>().ReverseMap();
        }
    }
}
