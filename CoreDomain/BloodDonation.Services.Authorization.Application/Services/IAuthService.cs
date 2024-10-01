using BloodDonation.Services.Authorization.Application.InputModels;
using Shared.Domain.Results;

namespace BloodDonation.Services.Authorization.Application.Services
{
    public interface IAuthService
    {
        Task<Result> ChangePasswordAsync(ChangePasswordInputModel model);
        Task<Result<string>> GeneratePasswordResetTokenAsync(GeneratePasswordResetTokenInputModel model);
        Task<Result<string>> LoginAsync(LoginInputModel model);
        Task<Result> RegisterAsync(RegisterInputModel model);
        Task<Result> ResetPasswordAsync(ResetPasswordInputModel model);
    }
}
