using System;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class SaleItemCancelDTO
    {
        [Required]
        public Guid Id { get; set; }

        public bool Cancelled { get; set; }
    }
}
