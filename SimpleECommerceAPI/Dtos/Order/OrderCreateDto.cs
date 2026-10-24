using SimpleECommerceAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Order
{
    public record OrderCreateDto([Required] Guid CartId, [EnumDataType(typeof(PaymentMethod))] PaymentMethod PaymentMethod);
}
