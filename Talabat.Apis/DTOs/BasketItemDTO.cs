﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.Apis.DTOs
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Range(0.1,double.MaxValue , ErrorMessage = "Price Cannot Be Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must Be One at Leaset")]
        public int Quantity { get; set; }
    }
}