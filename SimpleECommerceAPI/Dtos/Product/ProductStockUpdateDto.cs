using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Product
{
    public record ProductStockUpdateDto([Range(0, int.MaxValue)]int NewStock);
}
