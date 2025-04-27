using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Exceptions;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Features.Movie.Commands.Create;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Commands.Update {
    public class MovieUpdateCommand : IRequest<Response<MovieResponseDto>> {
        public int Id { get; set; }
        public CreateUpdateMovieDto MovieDto { get; set; }

        public MovieUpdateCommand(CreateUpdateMovieDto movieDto) {
            MovieDto = movieDto;
        }
    }

    public class MovieUpdateCommandHandler : IRequestHandler<MovieUpdateCommand, Response<MovieResponseDto>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<MovieResponseDto>> Handle(MovieUpdateCommand request, CancellationToken cancellationToken) {
            var validator = new MovieUpdateValidator();
            await validator.ValidateRequest(request.Id, request.MovieDto);

            var data = await _unitOfWork.Movie.GetByIdAsNoTrackingAsync(request.Id);
            if (data == null) {
                throw new NotFoundException($"Movie with ID: {request.Id} is Not Found");
            }

            var result = await _unitOfWork.Movie.UpdateMovieAsync(request.Id, request.MovieDto);
            var response = _mapper.Map<MovieResponseDto>(result);

            return new Response<MovieResponseDto>(response, "Movie Updated Successfully");
        }
    }
}
