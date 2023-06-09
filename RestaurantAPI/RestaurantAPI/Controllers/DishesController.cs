﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Data;
using RestaurantAPI.Helper;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : Controller
    {
        private readonly IDishRepository _repository;     

        public DishesController(IDishRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDishes([FromQuery] Pagination pagination)
        {
            var dishes = await _repository.GetAllAsync(pagination);
            return Ok(dishes);
        }

        [HttpGet]
        public async Task<IActionResult> GetDishByName(string name)
        {
            var dish = await _repository.GetByName(name);
            return Ok(dish);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewDish(Dish dish)
        {
            dish.Id = Guid.NewGuid();
            await _repository.AddNewDish(dish);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateDish(Dish dish)
        {
            var dishToUpdate = await _repository.GetByName(dish.Name);

            if (dishToUpdate == null)
                BadRequest();

            await _repository.UpdateDish(dishToUpdate);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteDish(Dish dish)
        {
            var dishToDelete = await _repository.GetByName(dish.Name);

            if (dishToDelete == null)
                BadRequest();

            await _repository.DeleteDish(dishToDelete);

            return Ok();
        }





    }
}
