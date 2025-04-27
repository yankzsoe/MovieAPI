using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Application.Interfaces {
    public interface IAuthService {
        Task<(string token, DateTime expires, UserDto dto)> RegisterAsync(RegisterDto dto);
        Task<(string token, DateTime expires, UserDto dto)> LoginAsync(LoginDto dto);
        (string token, DateTime expires) RefreshTokenAsync(ApplicationUser user);
        Task<string> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<string> ResetPasswordAsync(ResetPasswordDto dto);
        Task LogoutAsync();
    }
}
