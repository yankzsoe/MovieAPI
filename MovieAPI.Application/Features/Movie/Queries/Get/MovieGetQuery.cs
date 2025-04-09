using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Queries.Get {
    public class MovieGetQuery : IRequest<Response<MovieViewModel>> {
        public int Id { get; set; }
    }

    public class MovieGetQueryHandler : IRequestHandler<MovieGetQuery, Response<MovieViewModel>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieGetQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<MovieViewModel>> Handle(MovieGetQuery request, CancellationToken cancellationToken) {
            var data = await _unitOfWork.Movie.GetByIdAsNoTrackingAsync(request.Id);
            var result = _mapper.Map<MovieViewModel>(data);
            string message = result is not null ? "List of Movies has been sent succesfully" : "No Movie Found";
            return new Response<MovieViewModel>(result, message);
        }
    }
}
