namespace SimpleECommerceAPI.Dtos.Product
{
    public record ProductUpdateDto
    (
        string Name,
        string Description,
        decimal Price,
        int Stock,
        string ImageUrl,
        Guid CategoryId
    );
}
