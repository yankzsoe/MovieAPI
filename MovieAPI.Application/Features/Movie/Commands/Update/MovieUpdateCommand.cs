using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Exceptions;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Commands.Update {
    public class MovieUpdateCommand : IRequest<Response<string>> {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
    }

    public class MovieUpdateCommandHandler : IRequestHandler<MovieUpdateCommand, Response<string>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(MovieUpdateCommand request, CancellationToken cancellationToken) {
            var data = await _unitOfWork.Movie.GetAsync(request.Id);
            if (data == null) {
                throw new NotFoundException($"Movie with ID: {request.Id} Not Found");
            }

            data.Title = request.Title;
            data.Description = request.Description;
            data.Rating = request.Rating;
            data.Image = request.Image;

            await _unitOfWork.CompleteAsync();

            return new Response<string>("Movie Updated Successfully");
        }
    }
}
