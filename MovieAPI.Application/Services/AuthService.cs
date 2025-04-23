using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Application.Common.Exceptions;
using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Application.Interfaces;
using MovieAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Application.Services {
    public class AuthService : IAuthService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task RegisterAsync(RegisterDto dto) {
            var user = new ApplicationUser {
                UserName = dto.UserName,
                Email = dto.Email,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded) {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task<string> LoginAsync(string email, string password) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid email or password");
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new Exception("Invalid email or password");
            // Generate JWT Token
            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task LogoutAsync() {
            await _signInManager.SignOutAsync();
        }

        public string RefreshTokenAsync(ApplicationUser user) {
            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task<string> ForgotPasswordAsync(string email) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) {
                return null;
            }

            // Generate reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode token ke URL-safe format
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return await Task.FromResult(false);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (!result.Succeeded)
                return await Task.FromResult(false);

            return await Task.FromResult(true);
        }

        private string GenerateJwtToken(ApplicationUser user) {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
