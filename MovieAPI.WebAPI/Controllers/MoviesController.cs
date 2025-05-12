using Microsoft.AspNetCore.Mvc;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.Features.Movie.Commands.Create;
using MovieAPI.Application.Features.Movie.Commands.Update;
using MovieAPI.Application.Features.Movie.Queries.Get;
using MovieAPI.Application.Features.Movie.Queries.GetList;
using MovieAPI.Application.Features.Movie.Commands.Delete;
using AutoMapper;
using MovieAPI.Application.DTOs.Movie;
using Microsoft.AspNetCore.Authorization;
using MovieAPI.Application.Common.Models;

namespace MovieAPI.WebAPI.Controllers {
    [Route("api/[controller]")]
    public class MoviesController : ApiControllerBase {
        private readonly IMapper _mapper;
        public MoviesController(IMapper mapper) {
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyModel.GetMovie))]
        public async Task<ActionResult<PagedResponse<List<MovieResponseDto>>>> GetListAsync([FromQuery] MovieGetListQuery query) {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id:int}")]
        [ActionName(nameof(GetByIdAsync))] // handling error when call from CreatedAtAction
        public async Task<ActionResult<Response<MovieResponseDto>>> GetByIdAsync(int id) {
            var query = new MovieGetQuery { Id = id };
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyModel.CreateMovie))]
        public async Task<ActionResult> CreateAsync(CreateUpdateMovieDto dto) {
            var result = await Mediator.Send(new MovieCreateCommand(dto));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result }, dto);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = nameof(PolicyModel.DeleteMovie))]
        public async Task<ActionResult<Response<string>>> DeleteAsync(int id) {
            var command = new MovieDeleteCommand { Id = id };
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = nameof(PolicyModel.UpdateMovie))]
        public async Task<ActionResult<Response<MovieResponseDto>>> UpdateAsync(int id, [FromBody] CreateUpdateMovieDto request) {
            var command = new MovieUpdateCommand(request);
            command.Id = id;

            return Ok(await Mediator.Send(command));
        }
    }
}
