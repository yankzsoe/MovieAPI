using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieAPI.Application.Features.Movie.Commands.Update;
using MovieAPI.Application.Interfaces;
using MovieAPI.Application.Common.Exceptions;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Domain.Entities;

namespace MovieAPI.Tests.Movie.Commands.Update {
    public class MovieUpdateCommandHandlerTests {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MovieUpdateCommandHandler _handler;

        public MovieUpdateCommandHandlerTests() {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new MovieUpdateCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidMovieUpdate_ShouldUpdateAndReturnResponse() {
            // Arrange
            var movieId = 1;
            var existingMovie = new Domain.Entities.Movie {
                Id = movieId,
                Title = "Old Title",
                Description = "Old Description",
                Rating = 3.0f,
                Image = "old_image.jpg"
            };

            var movieDto = new CreateUpdateMovieDto {
                Title = "New Title",
                Description = "New Description",
                Rating = 4.5f,
                Image = "updated_image.jpg"
            };

            var updatedMovie = new MovieResponseDto {
                Id = movieId,
                Title = movieDto.Title,
                Description = movieDto.Description,
                Rating = movieDto.Rating,
                Image = movieDto.Image
            };

            var command = new MovieUpdateCommand(movieDto) { Id = movieId };

            _mockUnitOfWork.Setup(u => u.Movie.GetAsync(movieId))
                .ReturnsAsync(existingMovie);
            _mockUnitOfWork.Setup(u => u.Movie.UpdateMovieAsync(movieId, movieDto))
                .ReturnsAsync(new Domain.Entities.Movie {
                    Id = movieId,
                    Title = movieDto.Title,
                    Description = movieDto.Description,
                    Rating = movieDto.Rating,
                    Image = movieDto.Image
                });
            _mockMapper.Setup(m => m.Map<MovieResponseDto>(It.IsAny<Domain.Entities.Movie>()))
                .Returns(updatedMovie);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal("Movie Updated Successfully", result.Message);
            Assert.Equal(movieDto.Title, result.Data.Title);
            Assert.Equal(movieDto.Description, result.Data.Description);
            Assert.Equal(movieDto.Rating, result.Data.Rating);
            Assert.Equal(movieDto.Image, result.Data.Image);

            _mockUnitOfWork.Verify(u => u.Movie.GetAsync(movieId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Movie.UpdateMovieAsync(movieId, movieDto), Times.Once);
            _mockMapper.Verify(m => m.Map<MovieResponseDto>(It.Is<Domain.Entities.Movie>(m => m.Id == movieId)), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingMovie_ShouldThrowNotFoundException() {
            // Arrange
            var movieId = 99;
            var movieDto = new CreateUpdateMovieDto {
                Title = "no movie",
                Description = "no movie",
                Rating = 0,
                Image = "none.jpg"
            };

            var command = new MovieUpdateCommand(movieDto) { Id = movieId };

            _mockUnitOfWork.Setup(u => u.Movie.GetAsync(movieId))
                .ReturnsAsync((Domain.Entities.Movie)null); // Simulasi movie tidak ditemukan

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _mockUnitOfWork.Verify(u => u.Movie.GetAsync(movieId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Movie.UpdateMovieAsync(It.IsAny<int>(), It.IsAny<CreateUpdateMovieDto>()), Times.Never);
            _mockMapper.Verify(m => m.Map<MovieResponseDto>(It.IsAny<Domain.Entities.Movie>()), Times.Never);
        }


        [Fact]
        public async Task Handle_InvalidRequest_ShouldThrowValidationException() {
            // Arrange
            var movieId = 1;
            var invalidMovieDto = new CreateUpdateMovieDto {
                Title = "", // Invalid
                Description = "", // Invalid
                Rating = 0f,
                Image = "some-image.jpg"
            };

            var command = new MovieUpdateCommand(invalidMovieDto) { Id = 0 }; // Id kosong = invalid

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            _mockUnitOfWork.Verify(u => u.Movie.GetAsync(It.IsAny<int>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Movie.UpdateMovieAsync(It.IsAny<int>(), It.IsAny<CreateUpdateMovieDto>()), Times.Never);
            _mockMapper.Verify(m => m.Map<MovieResponseDto>(It.IsAny<Domain.Entities.Movie>()), Times.Never);
        }
    }
}
