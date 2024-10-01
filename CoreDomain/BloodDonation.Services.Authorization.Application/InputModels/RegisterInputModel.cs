using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodDonation.Services.Authorization.Application.InputModels
{
    public class RegisterInputModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int DonorId { get; set; }
    }
}
