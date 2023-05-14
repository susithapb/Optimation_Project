using EntityFrameworkCoreMock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Controllers;
using RestaurantAPI.Data;
using RestaurantAPI.Helper;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;
using RestaurantAPI.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace ResturantAPI.Tests
{
    public class DishControllerTests
    {

        private DishesController? _controller;
        private DbContextMock<DishDbContext>? _dbContextMock;
        private DbContextOptions? _options;
        private IDishRepository? _repository;

        public DbContextMock<DishDbContext> GetDbContext(Dish[] seedData)
        {

            DbContextMock<DishDbContext> dbContextMock = new DbContextMock<DishDbContext>(new DbContextOptionsBuilder<RestaurantAPI.Data.DishDbContext>().Options);
            dbContextMock.CreateDbSetMock(x => x.Dishes, seedData);
            return dbContextMock;
        }

        private DishDbContext DishesDbContextInit(DbContextMock<DishDbContext> dbContextMock)
        {
            return new DishDbContext(_options, dbContextMock.Object);
        }

        private DishesController DishesControllerInit(IDishRepository repository)
        {
            _repository = repository;
            return new DishesController(_repository);
        }

        private Dish[] GetInitialDbEntities()
        {
            return new Dish[]
             {
                new Dish() { Id = Guid.Parse("fae5eb71-a6f1-4945-8349-d3053b281aea"), Name = "Spaghetti bolognese", Description = "Spaghetti bolognese", Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="), Price = 16, Rating = 4.5},
                new Dish() { Id = Guid.Parse("3863ff5c-af17-487e-a001-7e01ff0d921b"), Name = "Boeuf Bourguignon", Description = "Boeuf Bourguignon", Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="), Price = 22, Rating = 4.3 },
                new Dish() { Id = Guid.Parse("637076d4-dea4-4c91-ad99-8ba7a8cedc4f"), Name = "Bouillabaisse", Description = "Bouillabaisse",Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="), Price = 25, Rating = 4.2 },
                new Dish() { Id = Guid.Parse("5e348d74-999b-454d-960a-92ad75f7661d"), Name = "French onion soup", Description = "French onion soup", Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="), Price = 18, Rating = 4.1 }
            };
        }

        private void Setup()
        {
            _dbContextMock = GetDbContext(GetInitialDbEntities());
            _controller = DishesControllerInit(_repository);
        }

        [Fact]
        public void GetAllDishes_Positive_ReturnAllDishes()
        {
            //arrange
            Setup();

            //act
            var result = _controller.GetAllDishes(new Pagination(1,10)).Result;
            OkObjectResult objectResponse = Assert.IsType<OkObjectResult>(result);

            List<Dish>? value = objectResponse.Value as List<Dish>;

            //assert
            Assert.Equal(4, value?.Count);

        }

        [Fact]
        public void GetDishByName_Positive_ReturnRelatedDish()
        {
            //arrange
            Setup();

            //act
            var result = _controller.GetDishByName("Spaghetti bolognese").Result;
            OkObjectResult objectResponse = Assert.IsType<OkObjectResult>(result);

            Dish? value = objectResponse.Value as Dish;

            //assert
            Assert.Equal("Spaghetti bolognese", value?.Name);

        }

        [Fact]
        public void GetDishByName_Negative_ReturnBadRequest()
        {
            //arrange
            Setup();

            //act
            var result = _controller.GetDishByName("Tarte Tatin").Result;

            //assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public void UpdateDish_Positive_ReturnOkResult()
        {
            //arrange
            Setup();

            Dish dishToUpdate = GetInitialDbEntities()[2];

            dishToUpdate.Name = "Chocolate soufflé";
            dishToUpdate.Description = "Chocolate soufflé";

            //act
            var result = _controller.UpdateDish(dishToUpdate).Result;
            Dish? updatedDish = _dbContextMock.Object.Dishes.Find(Guid.Parse("637076d4-dea4-4c91-ad99-8ba7a8cedc4f"));
            //assert
            Assert.Equal(dishToUpdate.Name, updatedDish?.Name);
            Assert.Equal(dishToUpdate.Description, updatedDish?.Description);

        }

        [Fact]
        public void UpdateCourse_Negative_ReturnBadResult()
        {
            //arrange
            Setup();

            Dish DishToUpdate = GetInitialDbEntities()[2];

            DishToUpdate.Name = "Crepes";
          
            //act
            var result = _controller.UpdateDish(DishToUpdate).Result;

            //assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public void DeleteDish_Positive_ReturnOkResult()
        {
            //arrange
            Setup();

            //act
            var result = _controller.DeleteDish(GetInitialDbEntities()[2]).Result;

            //assert
            Assert.Null(_dbContextMock.Object.Dishes.Find(GetInitialDbEntities()[2]));

        }

        [Fact]
        public void DeleteDish_Negative_ReturnBadResult()
        {
            //arrange
            Setup();

            //act
            var result = _controller.DeleteDish(new Dish()).Result;

            //assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public void AddCourse_Negative_ReturnBadResult()
        {
            //arrange
            Setup();

            Dish newCourse = new Dish()
            {
                Id = Guid.Parse("89873496-2d1c-4261-9028-b15ba5bb7fa8"),
                Name = "Cassoulet",
                Description = "Cassoulet",
                Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="),
                Price = 16,
                Rating = 4.3
            };

            //act
            var result = _controller.AddNewDish(newCourse).Result;

            //assert
            Assert.Equal(newCourse, _dbContextMock.Object.Dishes.Find(newCourse.Id));

        }
    }
}