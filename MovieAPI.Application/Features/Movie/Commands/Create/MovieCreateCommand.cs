using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Commands.Create {
    public class MovieCreateCommand : IRequest<Response<string>> {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
    }

    public class MovieCreateCommandHandler : IRequestHandler<MovieCreateCommand, Response<string>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(MovieCreateCommand request, CancellationToken cancellationToken) {
            var movie = new Domain.Entities.Movie() {
                Title = request.Title,
                Description = request.Description,
                Rating = request.Rating,
                Image = request.Image,
            };

            await _unitOfWork.Movie.AddAsync(movie);
            await _unitOfWork.CompleteAsync();
            return new Response<string>("Created Successfully");
        }
    }
}
