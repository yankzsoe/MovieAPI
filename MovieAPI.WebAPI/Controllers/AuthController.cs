using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Application.Common.Models.Responses;
using MovieAPI.Application.DTOs.Auth;
using MovieAPI.Application.Features.Auth.Commands;
using MovieAPI.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieAPI.WebAPI.Controllers {
    [Route("api/[controller]")]
    public class AuthController : ApiControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDto model) {
            var result = new ResgisterCommand(model);
            return Ok(await Mediator.Send(result));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto model) {
            var result = new LoginCommand(model);
            return Ok(await Mediator.Send(result));
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<Response<string>>> ForgotPassword([FromBody] ForgotPasswordDto model) {
            var result = new ForgotPasswordCommand(model);
            return Ok(await Mediator.Send(result));
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<Response<string>>> ResetPassword([FromBody] ResetPasswordDto model) {
            var result = new ResetPasswordCommand(model);
            return Ok(await Mediator.Send(result));
        }

    }
}
