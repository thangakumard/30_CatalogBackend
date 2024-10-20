using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.DTOs
{
    public record ProductDto
    {
        public int ProductId { get; set; }

        [Required]
        public string Sku { get; set; } = string.Empty;

        [Required]
        public string ProductDescription { get; set; } = string.Empty;

        [Required]
        public string ProductCategory { get; set; } = string.Empty;

        [Required, Range(1, int.MaxValue)] public int Quantity { get; set; }

        [Required, DataType(DataType.Currency)] public decimal Price { get; set; }


    }
}
