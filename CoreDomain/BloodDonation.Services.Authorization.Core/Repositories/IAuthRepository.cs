﻿using BloodDonation.Services.Authorization.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Domain.Results;

namespace BloodDonation.Services.Authorization.Domain.Persistence.Repositories
{
    public interface IAuthRepository
    {
        Task<Result> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<Result<string>> GeneratePasswordResetTokenAsync(string email);
        Task<Result<string>> LoginAsync(string email, string password);
        Task<Result> RegisterAsync(string email, string password);
        Task<Result> ResetPasswordAsync(string email, string token, string newPassword);
    }
}