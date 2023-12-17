using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MovieAPI.WebAPI.Controllers {
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ApiControllerBase : ControllerBase {
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
