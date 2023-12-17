using Microsoft.AspNetCore.Mvc;
using MovieAPI.Application.Common.Models;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.Features.Movie.Commands.Create;
using MovieAPI.Application.Features.Movie.Commands.Update;
using MovieAPI.Application.Features.Movie.Queries.Get;
using MovieAPI.Application.Features.Movie.Queries.GetList;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Linq;
using MovieAPI.Application.Features.Movie.Commands.Delete;

namespace MovieAPI.WebAPI.Controllers {
    [Route("/Movies")]
    public class MoviesController : ApiControllerBase {

        [HttpGet("/")]
        public async Task<ActionResult<PagedResponse<List<MovieViewModel>>>> GetListMovie([FromQuery] MovieGetListQuery query) {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("/{ID:int}")]
        public async Task<ActionResult<PagedResponse<List<MovieViewModel>>>> GetMovie(int ID) {
            var query = new MovieGetQuery { Id = ID };
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult<Response<MovieViewModel>>> CreateMovie(MovieCreateCommand command) {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("/{ID:int}")]
        public async Task<ActionResult<Response<string>>> DeleteMovie(int ID) {
            var command = new MovieDeleteCommand { Id = ID };
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("/{ID:int}")]
        public async Task<ActionResult<Response<string>>> UpdateMovie(int ID, [FromBody] CreateUpdateMovie request) {

            var command = new MovieUpdateCommand() {
                Id = ID,
                Title = request.Title,
                Description = request.Description,
                Rating = request.Rating,
                Image = request.Image
            };

            return Ok(await Mediator.Send(command));
        }
    }
}
