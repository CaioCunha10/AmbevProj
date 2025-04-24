using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class SalePostDTO
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "A lista de itens da venda é obrigatória.")]
        [MinLength(1, ErrorMessage = "A venda deve conter pelo menos um item.")]
        public List<SaleItemPostDTO> Items { get; set; }
    }

}
