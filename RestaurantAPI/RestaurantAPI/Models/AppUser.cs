using Microsoft.AspNetCore.Identity;

namespace RestaurantAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string NickName { get; set; }
        public Address Address { get; set; }
    }
}
