using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Features.Movie.Queries.GetList;
using MovieAPI.Domain.Entities;

namespace MovieAPI.Application.Interfaces.Repositories {
    public interface IMovieRepository : IGenericRepository<Movie> {
        Task<Movie> GetByIdAsNoTrackingAsync(int id);
        Task<(int totalCount, List<Movie> movies)> GetListByTitleAsNoTrackingAsync(string title);
        Task<(int totalCount, List<Movie> movies)> GetListAsNoTrackingAsync(MovieGetListQuery query);
        Task<Movie> UpdateMovieAsync(int id, MovieResponseDto movie);
    }
}
