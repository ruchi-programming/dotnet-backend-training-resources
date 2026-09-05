namespace ProductApi.Models;

public sealed record CreateProductRequest(
    string Name,
    decimal Price);
