using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
