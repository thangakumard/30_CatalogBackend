using System.ComponentModel.DataAnnotations;

namespace Orders.Application.DTOs
{
    public record OrderDto(
        int OrderId,
        [Required, Range(1, int.MaxValue)] int ProductId,
        [Required, Range(1, int.MaxValue)] int ClientId,
        [Required, Range(1, int.MaxValue)] int PurchaseQuantity,
        DateTime OrderDate
        );
}
