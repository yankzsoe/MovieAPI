using Moq;
using AutoMapper;
using MovieAPI.Application.Features.Movie.Commands.Create;
using MovieAPI.Application.Interfaces;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Common.Exceptions;

namespace MovieAPI.Tests.Movie.Commands.Create {
    public class MovieCreateCommandHandlerTests {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MovieCreateCommandHandler _handler;

        public MovieCreateCommandHandlerTests() {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new MovieCreateCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidMovieDto_ShouldCreateMovieAndReturnId() {
            // Arrange  
            var movieDto = new CreateUpdateMovieDto {
                Title = "Test Movie",
                Description = "Test Description",
                Rating = 4.5f,
                Image = "img path"
            };

            var movieEntity = new Domain.Entities.Movie {
                Id = 1,
                Title = movieDto.Title,
                Description = movieDto.Description,
                Rating = movieDto.Rating,
            };

            _mockMapper.Setup(m => m.Map<Domain.Entities.Movie>(It.IsAny<CreateUpdateMovieDto>())).Returns(movieEntity);
            _mockUnitOfWork.Setup(u => u.Movie.AddAsync(It.IsAny<Domain.Entities.Movie>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.FromResult(1));

            var command = new MovieCreateCommand(movieDto);

            // Act  
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert  
            Assert.Equal(movieEntity.Id, result);
            _mockMapper.Verify(m => m.Map<Domain.Entities.Movie>(It.Is<CreateUpdateMovieDto>(dto => dto == movieDto)), Times.Once);
            _mockUnitOfWork.Verify(u => u.Movie.AddAsync(It.Is<Domain.Entities.Movie>(m => m == movieEntity)), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidMovieDto_ShouldThrowValidationException() {
            // Arrange  
            var movieDto = new CreateUpdateMovieDto {
                Title = "", // Invalid input
                Description = "",
            };

            var command = new MovieCreateCommand(movieDto);

            // Act & Assert  
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
