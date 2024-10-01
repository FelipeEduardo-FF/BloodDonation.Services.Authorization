using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodDonation.Services.Authorization.Application.InputModels
{
    public class GeneratePasswordResetTokenInputModel
    {
        public string Email { get; set; } = string.Empty;
    }
}
