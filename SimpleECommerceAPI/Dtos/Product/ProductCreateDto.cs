namespace SimpleECommerceAPI.Dtos.Product
{
    public record ProductCreateDto
    (
        string Name, 
        string Description,
        decimal Price,
        int Stock,
        string ImageUrl,
        Guid CategoryId
    );
}
