using AutoMapper;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Features.Movie.Commands.Create;
using MovieAPI.Domain.Entities;

namespace MovieAPI.Application.Mappers {
    public class MovieMappingProfile : Profile {
        public MovieMappingProfile() {
            CreateMap<Movie, MovieViewModel>().ReverseMap();
                //.ForMember(d => d.Id, s => s.MapFrom(e => e.Id))
                //.ForMember(d => d.Title, s => s.MapFrom(e => e.Title))
                //.ForMember(d => d.Description, s => s.MapFrom(e => e.Description))
                //.ForMember(d => d.Rating, s => s.MapFrom(e => e.Rating))
                //.ForMember(d => d.Image, s => s.MapFrom(e => e.Image))
                //.ForMember(d => d.CreatedDate, s => s.MapFrom(e => e.CreatedDate))
                //.ForMember(d => d.UpdatedDate, s => s.MapFrom(e => e.UpdatedDate));

            //CreateMap<MovieViewModel, Movie>().ReverseMap();
                //.ForMember(d => d.Id, s => s.Ignore())
                //.ForMember(d => d.CreatedDate, s => s.Ignore())
                //.ForMember(d => d.UpdatedDate, s => s.Ignore());

            CreateMap<CreateUpdateMovie, Movie>().ReverseMap();
                //.ForMember(d => d.Title, s => s.MapFrom(e => e.Title))
                //.ForMember(d => d.Description, s => s.MapFrom(e => e.Description))
                //.ForMember(d => d.Rating, s => s.MapFrom(e => e.Rating))
                //.ForMember(d => d.Image, s => s.MapFrom(e => e.Image));

            CreateMap<MovieCreateCommand, Movie>().ReverseMap();
        }
    }
}
