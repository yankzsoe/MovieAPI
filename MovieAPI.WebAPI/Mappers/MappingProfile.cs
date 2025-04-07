using AutoMapper;

namespace MovieAPI.WebAPI.Mappers {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Application.Common.Models.CreateUpdateMovie, Application.Features.Movie.Commands.Update.MovieUpdateCommand>().ReverseMap();
        }
    }
}
