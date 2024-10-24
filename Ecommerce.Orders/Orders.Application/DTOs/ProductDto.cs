using System.ComponentModel.DataAnnotations;

namespace Orders.Application.DTOs
{
     public record ProductDto
    {
        public int ProductId { get; set; }

        [Required]
        public string Sku { get; set; } = string.Empty;

        [Required, Range(1, int.MaxValue)] public int Quantity { get; set; }

        [Required, DataType(DataType.Currency)] public decimal Price { get; set; }


    }
}
