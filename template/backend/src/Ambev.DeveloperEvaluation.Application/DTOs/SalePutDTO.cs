using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class SalePutDTO
    {
        [Required]
        public string SaleNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Customer { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Branch { get; set; }

        [Required]
        public List<SaleItemPutDTO> Items { get; set; }
    }

    public class SaleItemPutDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Product { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; }
    }
}
