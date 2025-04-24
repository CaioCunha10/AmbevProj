using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "O código do produto é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O código do produto deve ter no máximo 50 caracteres.")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque não pode ser negativa.")]
        public int StockQuantity { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [MaxLength(100, ErrorMessage = "A categoria deve ter no máximo 100 caracteres.")]
        public string Category { get; set; }
    }
}
