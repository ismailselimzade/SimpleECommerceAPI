namespace SimpleECommerceAPI.Dtos.Cart
{
    public record CartItemResponseDto
    (
        Guid Id,
        Guid CartId, 
        Guid ProductId, 
        string ProductName, 
        int Quantity, 
        decimal Price
    );
}
