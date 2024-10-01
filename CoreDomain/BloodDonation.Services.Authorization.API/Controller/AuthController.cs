using BloodDonation.Services.Authorization.API.Extensions;
using BloodDonation.Services.Authorization.Application.InputModels;
using BloodDonation.Services.Authorization.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Results;

namespace BloodDonation.Services.Authorization.API.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            var result = await _authService.LoginAsync(model);

            return result.Match(
                onSuccess: (token) => Ok(new { Token = token }),
                onFailure: (error) => error.ToProblemDetails()
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            var result = await _authService.RegisterAsync(model);

            return result.Match(
                onSuccess: NoContent,
                onFailure: (error) => error.ToProblemDetails()
            );
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            var result = await _authService.ChangePasswordAsync(model);

            return result.Match(
                onSuccess: NoContent,
                onFailure: (error) => error.ToProblemDetails()
            );
        }


        [HttpPost("generate-password-reset-token")]
        public async Task<IActionResult> GeneratePasswordResetToken(GeneratePasswordResetTokenInputModel model)
        {
            var result = await _authService.GeneratePasswordResetTokenAsync(model);

            return result.Match(
                onSuccess: (token) => Ok(new { ResetToken = token }),
                onFailure: (error) => error.ToProblemDetails()
            );
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordInputModel model)
        {
            var result = await _authService.ResetPasswordAsync(model);

            return result.Match(
                onSuccess: NoContent,
                onFailure: (error) => error.ToProblemDetails()
            );
        }
    }
}
