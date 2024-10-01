using BloodDonation.Services.Authorization.Application.InputModels;
using BloodDonation.Services.Authorization.Domain.Persistence.Repositories;
using Shared.Domain.Results;

namespace BloodDonation.Services.Authorization.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Result> ChangePasswordAsync(ChangePasswordInputModel model)
        {
            return await _authRepository.ChangePasswordAsync(model.Email, model.CurrentPassword, model.NewPassword);
        }

        public async Task<Result<string>> GeneratePasswordResetTokenAsync(GeneratePasswordResetTokenInputModel model)
        {
            return await _authRepository.GeneratePasswordResetTokenAsync(model.Email);
        }

        public async Task<Result<string>> LoginAsync(LoginInputModel model)
        {
            return await _authRepository.LoginAsync(model.Email, model.Password);
        }

        public async Task<Result> RegisterAsync(RegisterInputModel model)
        {
            return await _authRepository.RegisterAsync(model.Email, model.Password, model.DonorId);
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordInputModel model)
        {
            return await _authRepository.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
        }
    }

}
