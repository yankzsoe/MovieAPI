using MediatR;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Auth.Commands {
    public class ForgotPasswordCommand : IRequest<Response<string>> {
        public ForgotPasswordDto ForgotPasswordDto { get; set; }

        public ForgotPasswordCommand(ForgotPasswordDto registerDto) {
            ForgotPasswordDto = registerDto;
        }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Response<string>> {
        private readonly IAuthService _authService;
        public ForgotPasswordCommandHandler(IAuthService authService) {
            _authService = authService;
        }
        public async Task<Response<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken) {
            var result = await _authService.ForgotPasswordAsync(request.ForgotPasswordDto);
            return new Response<string>(result);
        }
    }
}
