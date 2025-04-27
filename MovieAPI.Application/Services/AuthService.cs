using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Application.Common.Exceptions;
using MovieAPI.Application.Common.Models;
using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Application.Interfaces;
using MovieAPI.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieAPI.Application.Services {
    public class AuthService : IAuthService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IMapper mapper, IEmailService emailService) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<(string token, DateTime expires, UserDto dto)> RegisterAsync(RegisterDto dto) {
            var user = new ApplicationUser {
                UserName = dto.UserName,
                Email = dto.Email,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) {
                throw new ValidationException(result.Errors);
            }

            var token = GenerateJwtToken(user);
            var userDto = _mapper.Map<UserDto>(user);
            return (token.token, token.expires, userDto);
        }

        public async Task<(string token, DateTime expires, UserDto dto)> LoginAsync(LoginDto dto) {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password");
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                throw new ValidationException("Invalid email or password");

            var token = GenerateJwtToken(user);
            var userDto = _mapper.Map<UserDto>(user);
            return (token.token, token.expires, userDto);
        }

        public async Task LogoutAsync() {
            await _signInManager.SignOutAsync();
        }

        public (string token, DateTime expires) RefreshTokenAsync(ApplicationUser user) {
            var token = GenerateJwtToken(user);
            return (token.token, token.expires);
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto) {
            var frontendUrl = _configuration["FrontendUrl"];
            var user = await _userManager.FindByEmailAsync(dto.Email);
            //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) {
            //    return null;
            //}

            // Jangan beri tahu apakah user tidak ditemukan atau email belum dikonfirmasi
            if (user == null)
                return await Task.FromResult("If your email is registered, you will receive reset instructions shortly.");

            // Generate reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode token ke URL-safe format
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // Buat link reset password (harus pointing ke frontend-mu nanti)
            var resetLink = $"{frontendUrl}/reset-password?email={Uri.EscapeDataString(dto.Email)}&token={Uri.EscapeDataString(encodedToken)}";

            await _emailService.SendEmailAsync(new Email() {
                Addresses = new List<string>() { dto.Email },
                Subject = "Reset Password",
                Body = $"<p>If you requested a password reset, please click <a href='{resetLink}'>here</a> to proceed.</p>"
            });

            return await Task.FromResult("Forgot password email successfully sent.");
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto) {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return "Reset password failed. Please try again.";

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
                return "Reset password failed. Please try again.";

            return "Reset password successfully. You can now login with your new password.";
        }

        private (string token, DateTime expires) GenerateJwtToken(ApplicationUser user) {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);
            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
