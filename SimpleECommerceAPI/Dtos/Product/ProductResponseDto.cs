namespace SimpleECommerceAPI.Dtos.Product
{
    public record ProductResponseDto
    (
        Guid Id,
        Guid CategoryId,
        string Name,
        string CategoryName,
        string Description,
        decimal Price,
        int Stock,
        string ImageUrl,
        DateTime CreatedAt
    );
}
