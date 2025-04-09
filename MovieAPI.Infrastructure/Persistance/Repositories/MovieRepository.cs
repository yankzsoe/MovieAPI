using Microsoft.EntityFrameworkCore;
using MovieAPI.Application.Common.Models.Enums;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Features.Movie.Queries.GetList;
using MovieAPI.Application.Interfaces;
using MovieAPI.Application.Interfaces.Repositories;
using MovieAPI.Domain.Entities;

namespace MovieAPI.Infrastructure.Persistance.Repositories {
    public class MovieRepository : GenericRepository<MovieRepository>, IMovieRepository {
        public MovieRepository(AppDbContext context) : base(context) { }

        public AppDbContext AppDbContext {
            get {
                return Context as AppDbContext;
            }
        }

        public async Task<(int totalCount, List<Movie> movies)> GetListAsNoTrackingAsync(MovieGetListQuery query) {
            IQueryable<Movie> data = AppDbContext.Movies
                .Where(e => e.Title.ToLower().Contains(query.Keyword.ToLower()));

            if (query.MovieGetListOrderBy == MovieGetListOrderBy.Id) {
                if (query.SortBy == SortBy.Asc) {
                    data.OrderBy(e => e.Id);
                } else {
                    data.OrderByDescending(e => e.Id);
                }
            }

            if (query.MovieGetListOrderBy == MovieGetListOrderBy.Title) {
                if (query.SortBy == SortBy.Asc) {
                    data.OrderBy(e => e.Title);
                } else {
                    data.OrderByDescending(e => e.Title);
                }
            }

            if (query.MovieGetListOrderBy == MovieGetListOrderBy.CreatedDate) {
                if (query.SortBy == SortBy.Asc) {
                    data.OrderBy(e => e.CreatedDate);
                } else {
                    data.OrderByDescending(e => e.CreatedDate);
                }
            }

            int totalCount = await data.AsNoTracking().CountAsync();

            var list = await data
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return (totalCount, list);
        }

        public Task<(int totalCount, List<Movie> movies)> GetListByTitleAsNoTrackingAsync(string title) {
            throw new NotImplementedException();
        }

        public async Task<Movie> GetByIdAsNoTrackingAsync(int id) {
            return await AppDbContext.Movies
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Movie> UpdateMovieAsync(int id, MovieResponseDto movie) {
            var data = new Movie() {
                Id = id,
                Title = movie.Title,
                Description = movie.Description,
                Rating = movie.Rating,
                Image = movie.Image,
            };

            AppDbContext.Movies.Update(data);
            await AppDbContext.SaveChangesAsync();
            return data;
        }

        public new async Task<Movie> GetAsync(int id) {
            return await AppDbContext.Movies
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Movie entity) {
            await AppDbContext.Movies.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Movie> entities) {
            await AppDbContext.Movies.AddRangeAsync(entities);
        }

        public void Remove(Movie entity) {
            AppDbContext.Movies.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Movie> entities) {
            AppDbContext.Movies.RemoveRange(entities);
        }
    }
}
