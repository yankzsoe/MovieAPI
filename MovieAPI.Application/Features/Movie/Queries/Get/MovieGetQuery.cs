using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Queries.Get {
    public class MovieGetQuery : IRequest<Response<MovieResponseDto>> {
        public int Id { get; set; }
    }

    public class MovieGetQueryHandler : IRequestHandler<MovieGetQuery, Response<MovieResponseDto>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieGetQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<MovieResponseDto>> Handle(MovieGetQuery request, CancellationToken cancellationToken) {
            var data = await _unitOfWork.Movie.GetByIdAsNoTrackingAsync(request.Id);
            var result = _mapper.Map<MovieResponseDto>(data);
            string message = result is not null ? "The data has been sent succesfully" : "No Movie Found";
            return new Response<MovieResponseDto>(result, message);
        }
    }
}
