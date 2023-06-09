﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace RestaurantAPI.Models
{
    public class Dish
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Remote("CheckNameExist", "Dish", ErrorMessage = "This name already exists")]
        public string Name { get; set; }

        [Required]
        [Remote("CheckDescriptionExist", "Dish", ErrorMessage = "This description already exists")]
        public string Description { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        public double Rating { get; set; }
    }
}
