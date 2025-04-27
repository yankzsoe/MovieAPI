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

            var updateCommand = new MovieUpdateCommand {
                Id = movieId,
                Title = "New Title",
                Description = "New Description",
                Rating = 4.5f,
                Image = "new_image.jpg"
            };

            var updatedMovieResponse = new MovieResponseDto {
                Id = movieId,
                Title = updateCommand.Title,
                Description = updateCommand.Description,
                Rating = updateCommand.Rating,
                Image = updateCommand.Image
            };

            _mockUnitOfWork.Setup(u => u.Movie.GetAsync(movieId))
                .ReturnsAsync(existingMovie);
            _mockUnitOfWork.Setup(u => u.CompleteAsync())
                .Returns(Task.FromResult(1));
            _mockMapper.Setup(m => m.Map<MovieResponseDto>(It.IsAny<Domain.Entities.Movie>()))
                .Returns(updatedMovieResponse);

            // Act
            var result = await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal("Movie Updated Successfully", result.Message);
            Assert.Equal(updateCommand.Title, result.Data.Title);
            Assert.Equal(updateCommand.Description, result.Data.Description);
            Assert.Equal(updateCommand.Rating, result.Data.Rating);
            Assert.Equal(updateCommand.Image, result.Data.Image);

            _mockUnitOfWork.Verify(u => u.Movie.GetAsync(movieId), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
            _mockMapper.Verify(m => m.Map<MovieResponseDto>(It.Is<Domain.Entities.Movie>(m => m.Id == movieId)), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingMovie_ShouldThrowNotFoundException() {
            // Arrange
            var movieId = 99;
            var updateCommand = new MovieUpdateCommand {
                Id = movieId,
                Title = "Doesn't Matter",
                Description = "Doesn't Matter",
                Rating = 0,
                Image = "none.jpg"
            };

            _mockUnitOfWork.Setup(u => u.Movie.GetAsync(movieId))
                .ReturnsAsync((Domain.Entities.Movie)null); // Simulasi movie tidak ditemukan

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(updateCommand, CancellationToken.None));

            _mockUnitOfWork.Verify(u => u.Movie.GetAsync(movieId), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
            _mockMapper.Verify(m => m.Map<MovieResponseDto>(It.IsAny<Domain.Entities.Movie>()), Times.Never);
        }
    }
}
