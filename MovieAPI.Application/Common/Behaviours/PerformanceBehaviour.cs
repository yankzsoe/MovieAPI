using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MovieAPI.Application.Common.Behaviours {
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(ILogger<TRequest> logger) {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 3000) {
                var requestName = typeof(TRequest).Name;

                _logger.LogWarning("Long running request: {requestName} ({elapsedMilliseconds} milliseconds).",
                    requestName, elapsedMilliseconds);
            }

            return response;
        }
    }
}
