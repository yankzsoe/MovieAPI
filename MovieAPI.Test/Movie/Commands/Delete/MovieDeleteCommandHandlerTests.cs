using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieAPI.Application.Features.Movie.Commands.Delete;
using MovieAPI.Application.Interfaces;
using MovieAPI.Application.Common.Exceptions;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Domain.Entities;

namespace MovieAPI.Tests.Movie.Commands.Delete {
    public class MovieDeleteCommandHandlerTests {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MovieDeleteCommandHandler _handler;

        public MovieDeleteCommandHandlerTests() {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new MovieDeleteCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidMovieId_ShouldDeleteMovieAndReturnSuccessMessage() {
            // Arrange
            var movieId = 1;
            var existingMovie = new Domain.Entities.Movie {
                Id = movieId,
                Title = "Test Movie"
            };

            _mockUnitOfWork.Setup(u => u.Movie.GetAsync(movieId))
                .ReturnsAsync(existingMovie);
            _mockUnitOfWork.Setup(u => u.CompleteAsync())
                .Returns(Task.FromResult(1));

            var command = new MovieDeleteCommand { Id = movieId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal("Movie has been deleted successfully", result.Message);

            _mockUnitOfWork.Verify(u => u.Movie.GetAsync(movieId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Movie.Remove(existingMovie), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingMovie_ShouldThrowNotFoundException() {
            // Arrange
            var movieId = 99;

            _mockUnitOfWork.Setup(u => u.Movie.GetAsync(movieId))
                .ReturnsAsync((Domain.Entities.Movie)null); // Simulasi tidak ketemu

            var command = new MovieDeleteCommand { Id = movieId };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _mockUnitOfWork.Verify(u => u.Movie.GetAsync(movieId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Movie.Remove(It.IsAny<Domain.Entities.Movie>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}
