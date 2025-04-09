using AutoMapper;
using MovieAPI.Application.DTOs.Movie;

namespace MovieAPI.WebAPI.Mappers {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CreateUpdateMovieDto, Application.Features.Movie.Commands.Update.MovieUpdateCommand>().ReverseMap();
        }
    }
}
