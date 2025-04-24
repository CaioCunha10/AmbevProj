using System;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class ProductUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        public string Description { get; set; }

    }
}
