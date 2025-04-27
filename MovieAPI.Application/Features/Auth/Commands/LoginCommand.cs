using MediatR;
using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Auth.Commands {
    public class LoginCommand : IRequest<AuthResponse> {
        public LoginDto LoginDto { get; set; }

        public LoginCommand(LoginDto dto) {
            LoginDto = dto;
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse> {
        private readonly IAuthService _authService;
        public LoginCommandHandler(IAuthService authService) {
            _authService = authService;
        }
        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken) {
            var token = await _authService.LoginAsync(request.LoginDto);

            return new AuthResponse("Bearer", token.token, token.expires, token.dto);
        }
    }
}
