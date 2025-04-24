using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "O c�digo do produto � obrigat�rio.")]
        [MaxLength(50, ErrorMessage = "O c�digo do produto deve ter no m�ximo 50 caracteres.")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "O nome � obrigat�rio.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no m�ximo 100 caracteres.")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O pre�o deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque n�o pode ser negativa.")]
        public int StockQuantity { get; set; }

        [MaxLength(500, ErrorMessage = "A descri��o deve ter no m�ximo 500 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A categoria � obrigat�ria.")]
        [MaxLength(100, ErrorMessage = "A categoria deve ter no m�ximo 100 caracteres.")]
        public string Category { get; set; }
    }
}
