using Microsoft.AspNetCore.Identity;

namespace BloodDonation.Services.Authorization.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int? DonorId { get; set; }
    }
}
