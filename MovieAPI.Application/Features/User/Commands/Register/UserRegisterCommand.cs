using MediatR;
using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Application.Features.User.Commands.Register {
    public class UserRegisterCommand : IRequest<AuthResponse> {
        public RegisterDto RegisterDto { get; set; }
        public UserRegisterCommand(RegisterDto dto) {
            RegisterDto = dto;
        }
    }

    public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, AuthResponse> {
        private readonly IAuthService _authService;

        public UserRegisterCommandHandler(IAuthService authService) {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(UserRegisterCommand request, CancellationToken cancellationToken) {
            await _authService.RegisterAsync(request.RegisterDto);

            throw new NotImplementedException();
        }
    }
}
