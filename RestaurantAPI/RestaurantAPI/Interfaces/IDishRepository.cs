using RestaurantAPI.Helper;
using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IDishRepository
    {
        Task<IEnumerable<Dish>> GetAllAsync(Pagination pagination);

        Task<Dish> GetByName(string name);  

        Task<Dish> AddNewDish(Dish dish);

        Task<Dish> UpdateDish(Dish dish);
        Task DeleteDish(Dish dish);
    }
}
