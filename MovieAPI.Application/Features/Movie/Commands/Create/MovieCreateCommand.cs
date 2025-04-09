using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.DTOs.Movie;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Commands.Create {
    public class MovieCreateCommand : IRequest<Response<MovieViewModel>> {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
    }

    public class MovieCreateCommandHandler : IRequestHandler<MovieCreateCommand, Response<MovieViewModel>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<MovieViewModel>> Handle(MovieCreateCommand request, CancellationToken cancellationToken) {
            //var movie = new Domain.Entities.Movie() {
            //    Title = request.Title,
            //    Description = request.Description,
            //    Rating = request.Rating,
            //    Image = request.Image,
            //};

            var movie = _mapper.Map<Domain.Entities.Movie>(request);

            await _unitOfWork.Movie.AddAsync(movie);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<MovieViewModel>(movie);
            return new Response<MovieViewModel>(response, "Created Successfully");
        }
    }
}
