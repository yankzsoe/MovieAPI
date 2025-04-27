using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieAPI.Application.Features.Movie.Queries.GetList;
using MovieAPI.Application.Interfaces;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Domain.Entities;

namespace MovieAPI.Tests.Movie.Queries.GetList {
    public class MovieGetListQueryHandlerTests {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MovieGetListQueryHandler _handler;

        public MovieGetListQueryHandlerTests() {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new MovieGetListQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_MoviesExist_ShouldReturnPagedMovieList() {
            // Arrange
            var movies = new List<Domain.Entities.Movie> {
                new Domain.Entities.Movie { Id = 1, Title = "Movie 1", Description = "Desc 1", Rating = 4.0f, Image = "img1.jpg" },
                new Domain.Entities.Movie { Id = 2, Title = "Movie 2", Description = "Desc 2", Rating = 3.5f, Image = "img2.jpg" }
            };

            var movieDtos = new List<MovieResponseDto> {
                new MovieResponseDto { Id = 1, Title = "Movie 1", Description = "Desc 1", Rating = 4.0f, Image = "img1.jpg" },
                new MovieResponseDto { Id = 2, Title = "Movie 2", Description = "Desc 2", Rating = 3.5f, Image = "img2.jpg" }
            };

            var query = new MovieGetListQuery {
                Keyword = "Movie",
                PageNumber = 1,
                PageSize = 10
            };

            _mockUnitOfWork.Setup(u => u.Movie.GetListAsNoTrackingAsync(query))
                .ReturnsAsync((movies.Count, movies));
            _mockMapper.Setup(m => m.Map<List<MovieResponseDto>>(movies))
                .Returns(movieDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal("List of Movies has been sent succesfully", result.Message);
            Assert.Equal(movies.Count, result.Data.Count);
            Assert.Equal(movies.Count, result.Pagination.TotalCount);
            Assert.Equal(query.PageSize, result.Pagination.PageSize);
            Assert.Equal(query.PageNumber, result.Pagination.CurrentPage);

            _mockUnitOfWork.Verify(u => u.Movie.GetListAsNoTrackingAsync(query), Times.Once);
            _mockMapper.Verify(m => m.Map<List<MovieResponseDto>>(movies), Times.Once);
        }

        [Fact]
        public async Task Handle_NoMoviesFound_ShouldReturnEmptyList() {
            // Arrange
            var movies = new List<Domain.Entities.Movie>(); // Empty list

            var movieDtos = new List<MovieResponseDto>(); // Empty DTO list

            var query = new MovieGetListQuery {
                Keyword = "NonExistingMovie",
                PageNumber = 1,
                PageSize = 10
            };

            _mockUnitOfWork.Setup(u => u.Movie.GetListAsNoTrackingAsync(query))
                .ReturnsAsync((0, movies));
            _mockMapper.Setup(m => m.Map<List<MovieResponseDto>>(movies))
                .Returns(movieDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal("No Movie Found", result.Message);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.Data.Count);
            Assert.Equal(query.PageSize, result.Pagination.PageSize);
            Assert.Equal(query.PageNumber, result.Pagination.CurrentPage);

            _mockUnitOfWork.Verify(u => u.Movie.GetListAsNoTrackingAsync(query), Times.Once);
            _mockMapper.Verify(m => m.Map<List<MovieResponseDto>>(movies), Times.Once);
        }
    }
}
