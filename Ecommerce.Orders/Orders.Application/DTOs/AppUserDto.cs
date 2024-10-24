using System.ComponentModel.DataAnnotations;

namespace Orders.Application.DTOs
{
    public record AppUserDto(
           int UserId,
           [Required] string UserName,
           [Required] string PhoneNumber,
           [Required] string Address,
           [Required, EmailAddress] string Email,
           [Required] string Password,
           [Required] string Role
       );
}
