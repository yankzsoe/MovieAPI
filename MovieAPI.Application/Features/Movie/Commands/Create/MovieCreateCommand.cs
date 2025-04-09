using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Commands.Create {
    public class MovieCreateCommand : IRequest<int> {
        public CreateUpdateMovieDto MovieDto { get; set; }

        public MovieCreateCommand(CreateUpdateMovieDto dto) {
            MovieDto = dto;
        }
    }

    public class MovieCreateCommandHandler : IRequestHandler<MovieCreateCommand, int> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(MovieCreateCommand request, CancellationToken cancellationToken) {
            var movie = _mapper.Map<Domain.Entities.Movie>(request.MovieDto);
            await _unitOfWork.Movie.AddAsync(movie);
            await _unitOfWork.CompleteAsync();
            return movie.Id;
        }
    }
}
