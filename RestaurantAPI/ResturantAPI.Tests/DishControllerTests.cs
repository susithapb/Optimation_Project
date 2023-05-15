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

        private DishRepository _repository;
        public static DbContextOptions<DishDbContext> dbContextOptions { get; }
        DbContextMock<DishDbContext>? _dbContextMock;

        public DbContextMock<DishDbContext> GetDbContext(Dish[] seedData)
        {

            DbContextMock<DishDbContext> dbContextMock = new DbContextMock<DishDbContext>(new DbContextOptionsBuilder<DishDbContext>().Options);
            dbContextMock.CreateDbSetMock(x => x.Dishes, seedData);
            return dbContextMock;
        }

        private Dish[] GetInitialDbEntities()
        {
            return new Dish[]
             {
                new Dish() { Id = Guid.Parse("fae5eb71-a6f1-4945-8349-d3053b281aea"), Name = "Spaghetti bolognese", Description = "Spaghetti bolognese", Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="), Price = 16, Rating = 4.5 },
                new Dish() { Id = Guid.Parse("3863ff5c-af17-487e-a001-7e01ff0d921b"), Name = "Boeuf Bourguignon", Description = "Boeuf Bourguignon", Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="), Price = 22, Rating = 4.3 },
                new Dish() { Id = Guid.Parse("637076d4-dea4-4c91-ad99-8ba7a8cedc4f"), Name = "Bouillabaisse", Description = "Bouillabaisse", Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="), Price = 25, Rating = 4.2 },
                new Dish() { Id = Guid.Parse("5e348d74-999b-454d-960a-92ad75f7661d"), Name = "French onion soup", Description = "French onion soup", Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="), Price = 18, Rating = 4.1 }
           
            };
        }

        private void Setup()
        {
            _dbContextMock = GetDbContext(GetInitialDbEntities());
            _repository = DishRepositoryInit(_dbContextMock);
        }

        private DishRepository DishRepositoryInit(DbContextMock<DishDbContext> dbContextMock)
        {
            return new DishRepository(dbContextMock.Object);
        }

        [Fact]
        public void GetAllDishes_Positive_ReturnOkResult()
        {
            Setup();

            var result = _repository.GetAllAsync(new Pagination(1, 10)).Result;
            
            //assert
            Assert.Equal(4, result.Count);


        }


        [Fact]
        public void GetDishByName_Positive_ReturnRelatedDish()
        {
            //arrange
            Setup();

            //act
            var result = _repository.GetByName("Spaghetti bolognese").Result;
           
         
            //assert
            Assert.Equal("Spaghetti bolognese", result?.Name);

        }

        [Fact]
        public void GetDishByName_Negative_ReturnBadRequest()
        {
            //arrange
            Setup();

            //act & assert
            Assert.ThrowsAsync<Exception>(() => _repository.GetByName("Tarte Tatin"));

        }

        [Fact]
        public void UpdateDish_Positive_ReturnOkResult()
        {
            //arrange
            Setup();

            Dish dishToUpdate = GetInitialDbEntities()[2];

            dishToUpdate.Price = 32;            

            //act
            var result = _repository.UpdateDish(dishToUpdate).Result;
            Dish? updatedDish = _dbContextMock.Object.Dishes.Find(Guid.Parse("637076d4-dea4-4c91-ad99-8ba7a8cedc4f"));

            //assert
            Assert.Equal(dishToUpdate.Price, updatedDish?.Price);
            

        }

        [Fact]
        public void UpdateDishes_Negative_ReturnBadResult()
        {
            //arrange
            Setup();

            Dish DishToUpdate = GetInitialDbEntities()[2];

            DishToUpdate.Name = "Crepes";

            //act & assert
            Assert.ThrowsAsync<Exception>(() => _repository.UpdateDish(DishToUpdate));

        }

        [Fact]
        public void DeleteDish_Positive_ReturnOkResult()
        {
            //arrange
            Setup();

            //act
            var result = _repository.DeleteDish(GetInitialDbEntities()[2]);

            //assert
            Assert.Null(_dbContextMock.Object.Dishes.Find(GetInitialDbEntities()[2]));

        }

        [Fact]
        public void DeleteDish_Negative_ReturnBadResult()
        {
            //arrange
            Setup();

            //act & assert
            Assert.ThrowsAsync<Exception>(() => _repository.DeleteDish(new Dish()));

        }

        [Fact]
        public void AddDishes_Negative_ReturnBadResult()
        {
            //arrange
            Setup();

            Dish newDish = new Dish()
            {
                Id = Guid.Parse("89873496-2d1c-4261-9028-b15ba5bb7fa8"),
                Name = "Cassoulet",
                Description = "Cassoulet",
                Image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw=="),
                Price = 16,
                Rating = 4.3
            };

            //act
            var result = _repository.AddNewDish(newDish).Result;

            //assert
            Assert.Equal(newDish, _dbContextMock.Object.Dishes.Find(newDish.Id));

        }



    }
}