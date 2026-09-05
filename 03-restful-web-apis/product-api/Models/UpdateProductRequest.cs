namespace ProductApi.Models;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price);
