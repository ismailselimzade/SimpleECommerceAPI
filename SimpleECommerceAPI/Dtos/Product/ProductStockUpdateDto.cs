using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Product
{
    public record ProductStockUpdateDto([Required] [Range(0, int.MaxValue)]int NewStock);
}
