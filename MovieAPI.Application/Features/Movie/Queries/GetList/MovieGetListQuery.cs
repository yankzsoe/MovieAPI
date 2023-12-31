﻿using AutoMapper;
using MediatR;
using MovieAPI.Application.Common.Models;
using MovieAPI.Application.Common.Models.Enums;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Movie.Queries.GetList {
    public class MovieGetListQuery : PagedQuery, IRequest<PagedResponse<List<MovieViewModel>>> {
        /// <summary>
        /// Search by: Title
        /// </summary>
        public string Keyword { get; set; } = string.Empty;

        /// <summary>
        /// Default: Title
        /// </summary>
        public MovieGetListOrderBy MovieGetListOrderBy { get; set; } = MovieGetListOrderBy.Title;

        /// <summary>
        /// Default: Asc
        /// </summary>
        public new SortBy SortBy { get; set; } = SortBy.Asc;
    }

    public class MovieGetListQueryHandler : IRequestHandler<MovieGetListQuery, PagedResponse<List<MovieViewModel>>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieGetListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<MovieViewModel>>> Handle(MovieGetListQuery request, CancellationToken cancellationToken) {
            var (totalCount, list) = await _unitOfWork.Movie.GetListAsNoTrackingAsync(request);

            string message = totalCount > 0 ? "List of Movies has been sent succesfully" : "No Movie Found";
            var result = _mapper.Map<List<MovieViewModel>>(list);

            return new PagedResponse<List<MovieViewModel>>(result, totalCount, request.PageSize, request.PageNumber, message);
        }
    }
}
