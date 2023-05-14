using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Helper;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;
using XAct.Messages;

namespace RestaurantAPI.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly DishDbContext _dbContext;

        public DishRepository(DishDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Dish>> GetAllAsync([FromQuery] Pagination pagination)
        {
            var pagedData = await _dbContext.Dishes
                    .OrderBy(x => x.Name)
                    .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

            return pagedData;
        }

        public async Task<Dish> GetByName(string name)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(x => x.Name == name);
            if (dish == null)
                throw new Exception($"Dish {dish.Name} is not found.");

            return dish;
        }

        public async Task<Dish> AddNewDish(Dish dish)
        {
            await _dbContext.Dishes.AddAsync(dish);
            await _dbContext.SaveChangesAsync();

            return dish;
        }

        public async Task<Dish> UpdateDish(Dish dish)
        {
            var dishToUpdate = _dbContext.Dishes.FirstOrDefault(x => x.Name == dish.Name);

            if (dishToUpdate == null)
                throw new Exception($"Dish {dish.Name} is not found.");

            dishToUpdate.Name = dish.Name;
            dishToUpdate.Description = dish.Description;
            dishToUpdate.ImageUrl = dish.ImageUrl;
            dishToUpdate.Price = dish.Price;

            await _dbContext.SaveChangesAsync();

            return dishToUpdate;

        }

        public async Task DeleteDish(Dish dish)
        {
            var dishToDelete = _dbContext.Dishes.FirstOrDefault(x => x.Name == dish.Name);

            if(dishToDelete == null)
                throw new Exception($"Dish {dish.Name} is not found.");

            _dbContext.Set<Dish>().Remove(dishToDelete);

            await _dbContext.SaveChangesAsync();
        }        

    }
}
