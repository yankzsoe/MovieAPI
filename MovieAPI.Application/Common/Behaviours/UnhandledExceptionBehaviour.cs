using MediatR;
using Microsoft.Extensions.Logging;
using MovieAPI.Application.Common.Exceptions;

namespace MovieAPI.Application.Common.Behaviours {
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger) {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
            try {
                return await next();
            } catch (Exception ex) {
                if (ex is NotFoundException) throw;
                else if (ex is UnauthorizedException) throw;
                else if (ex is ValidationException) throw;
                else if (ex is ForbiddenException) throw;

                var requestName = typeof(TRequest).Name;

                _logger.LogError("Unhandled exception from {requestName}: {errorMessage}"
                    , requestName, ex.Message);

                throw;
            }
        }
    }
}
