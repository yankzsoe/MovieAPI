using MediatR;
using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Features.Auth.Commands {
    public class ResgisterCommand : IRequest<AuthResponse> {
        public RegisterDto RegisterDto { get; set; }

        public ResgisterCommand(RegisterDto registerDto) {
            RegisterDto = registerDto;
        }
    }

    public class ResgisterCommandHandler : IRequestHandler<ResgisterCommand, AuthResponse> {
        private readonly IAuthService _authService;
        public ResgisterCommandHandler(IAuthService authService) {
            _authService = authService;
        }
        public async Task<AuthResponse> Handle(ResgisterCommand request, CancellationToken cancellationToken) {
            var token = await _authService.RegisterAsync(request.RegisterDto);

            return new AuthResponse("Bearer", token.token, token.expires, token.dto);
        }
    }
}
