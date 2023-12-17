using AutoMapper;
using MovieAPI.Application.Common.Models;
using MovieAPI.Domain.Entities;

namespace MovieAPI.Application.Mappers {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Movie, MovieViewModel>()
                .ForMember(d => d.Id, s => s.MapFrom(e => e.Id))
                .ForMember(d => d.Title, s => s.MapFrom(e => e.Title))
                .ForMember(d => d.Description, s => s.MapFrom(e => e.Description))
                .ForMember(d => d.Rating, s => s.MapFrom(e => e.Rating))
                .ForMember(d => d.Image, s => s.MapFrom(e => e.Image))
                .ForMember(d => d.CreatedDate, s => s.MapFrom(e => e.CreatedDate))
                .ForMember(d => d.UpdateDate, s => s.MapFrom(e => e.UpdatedDate));

            CreateMap<MovieViewModel, Movie>()
                .ForMember(d => d.Id, s => s.Ignore())
                .ForMember(d => d.CreatedDate, s => s.Ignore())
                .ForMember(d => d.UpdatedDate, s => s.Ignore());

            CreateMap<CreateUpdateMovie, Movie>()
                .ForMember(d => d.Title, s => s.MapFrom(e => e.Title))
                .ForMember(d => d.Description, s => s.MapFrom(e => e.Description))
                .ForMember(d => d.Rating, s => s.MapFrom(e => e.Rating))
                .ForMember(d => d.Image, s => s.MapFrom(e => e.Image));
        }
    }
}
