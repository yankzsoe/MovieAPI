using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Application.Interfaces {
    public interface IAuthService {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(string email, string password);
        string RefreshTokenAsync(ApplicationUser user);
        Task<string> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task LogoutAsync();
    }
}
