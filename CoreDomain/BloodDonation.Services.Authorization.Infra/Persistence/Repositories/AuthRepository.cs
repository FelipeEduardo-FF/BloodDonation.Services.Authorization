using BloodDonation.Services.Authorization.Domain.Entities;
using BloodDonation.Services.Authorization.Domain.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Results;
using Shared.Domain.Results.Errors;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BloodDonation.Services.Authorization.Infra.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<Result> RegisterAsync(string email, string password)
        {
            var user = new ApplicationUser { UserName = email, Email = email};
            var result= await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(400, e.Description, ErrorType.Validation));
                return OperationResult.Fail(errors);
            }
            await _userManager.AddToRoleAsync(user!, "STAFF");

            return OperationResult.Ok();
        }

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                // Verifique as diferentes propriedades para descobrir o motivo do erro
                if (result.IsLockedOut)
                {
                    return OperationResult.Forbidden<string>("User is locked out.");
                }
                else if (result.IsNotAllowed)
                {
                    return OperationResult.Forbidden<string>("User is not allowed to sign in.");
                }
                else if (result.RequiresTwoFactor)
                {
                    return OperationResult.Forbidden<string>("User must use two-factor authentication.");
                }

                return OperationResult.Forbidden<string>("Invalid login attempt.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            return OperationResult.Ok(await GenerateJwtToken(user!));
        }


        public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return OperationResult.NotFound("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(400, e.Description, ErrorType.Validation));
                return OperationResult.Fail(errors);
            }

            return OperationResult.Ok();
        }

        public async Task<Result> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return OperationResult.NotFound("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(400, e.Description, ErrorType.Validation));
                return OperationResult.Fail(errors);
            }

            return OperationResult.Ok();
        }


        public async Task<Result<string>> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return OperationResult.NotFound<string>("User not found");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return OperationResult.Ok(token);
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }


}
