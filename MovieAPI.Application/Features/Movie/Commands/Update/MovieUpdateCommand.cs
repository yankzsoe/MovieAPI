﻿using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Exceptions;
using MovieAPI.Application.Common.Models;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Commands.Update {
    public class MovieUpdateCommand : IRequest<Response<MovieViewModel>> {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
    }

    public class MovieUpdateCommandHandler : IRequestHandler<MovieUpdateCommand, Response<MovieViewModel>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<MovieViewModel>> Handle(MovieUpdateCommand request, CancellationToken cancellationToken) {
            var data = await _unitOfWork.Movie.GetAsync(request.Id);
            if (data == null) {
                throw new NotFoundException($"Movie with ID: {request.Id} Not Found");
            }

            data.Title = request.Title;
            data.Description = request.Description;
            data.Rating = request.Rating;
            data.Image = request.Image;

            await _unitOfWork.CompleteAsync();
            var response = _mapper.Map<MovieViewModel>(data);

            return new Response<MovieViewModel>(response, "Movie Updated Successfully");
        }
    }
}
