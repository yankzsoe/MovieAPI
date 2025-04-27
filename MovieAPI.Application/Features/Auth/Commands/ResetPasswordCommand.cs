using MediatR;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Auth.Commands {
    public class ResetPasswordCommand : IRequest<Response<string>> {
        public ResetPasswordDto ResetPasswordDto { get; set; }

        public ResetPasswordCommand(ResetPasswordDto dto) {
            ResetPasswordDto = dto;
        }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Response<string>> {
        private readonly IAuthService _authService;
        public ResetPasswordCommandHandler(IAuthService authService) {
            _authService = authService;
        }
        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken) {
            var token = await _authService.ResetPasswordAsync(request.ResetPasswordDto);
            return new Response<string>(token);
        }
    }
}
