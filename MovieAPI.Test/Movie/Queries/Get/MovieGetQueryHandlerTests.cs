using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieAPI.Application.Features.Movie.Queries.Get;
using MovieAPI.Application.Interfaces;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Domain.Entities;

namespace MovieAPI.Tests.Movie.Queries.Get {
    public class MovieGetQueryHandlerTests {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MovieGetQueryHandler _handler;

        public MovieGetQueryHandlerTests() {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new MovieGetQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ExistingMovie_ShouldReturnMovieResponse() {
            // Arrange
            var movieId = 1;
            var existingMovie = new Domain.Entities.Movie {
                Id = movieId,
                Title = "Test Movie",
                Description = "Test Description",
                Rating = 4.5f,
                Image = "test.jpg"
            };

            var movieResponse = new MovieResponseDto {
                Id = movieId,
                Title = existingMovie.Title,
                Description = existingMovie.Description,
                Rating = existingMovie.Rating,
                Image = existingMovie.Image
            };

            _mockUnitOfWork.Setup(u => u.Movie.GetByIdAsNoTrackingAsync(movieId))
                .ReturnsAsync(existingMovie);
            _mockMapper.Setup(m => m.Map<MovieResponseDto>(existingMovie))
                .Returns(movieResponse);

            var query = new MovieGetQuery { Id = movieId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal("The data has been sent succesfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(movieId, result.Data.Id);

            _mockUnitOfWork.Verify(u => u.Movie.GetByIdAsNoTrackingAsync(movieId), Times.Once);
            _mockMapper.Verify(m => m.Map<MovieResponseDto>(existingMovie), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingMovie_ShouldReturnNoMovieFound() {
            // Arrange
            var movieId = 99;

            _mockUnitOfWork.Setup(u => u.Movie.GetByIdAsNoTrackingAsync(movieId))
                .ReturnsAsync((Domain.Entities.Movie)null);
            _mockMapper.Setup(m => m.Map<MovieResponseDto>(null))
                .Returns((MovieResponseDto)null);

            var query = new MovieGetQuery { Id = movieId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal("No Movie Found", result.Message);
            Assert.Null(result.Data);

            _mockUnitOfWork.Verify(u => u.Movie.GetByIdAsNoTrackingAsync(movieId), Times.Once);
            _mockMapper.Verify(m => m.Map<MovieResponseDto>(null), Times.Once);
        }
    }
}
