# RESTful Product API

## Learning objectives

After completing this example, students will be able to:

- Build a controller-based ASP.NET Core Web API.
- Map CRUD operations to HTTP methods.
- Use route constraints and model binding.
- Return appropriate HTTP status codes.
- Apply dependency inversion through an interface.
- Register and inject a service.
- Protect shared in-memory state from concurrent requests.

## API endpoints

| Method | Route | Purpose | Success status |
|---|---|---|---:|
| `GET` | `/api/products` | Get all products | `200` |
| `GET` | `/api/products/{id}` | Get one product | `200` |
| `POST` | `/api/products` | Create a product | `201` |
| `PUT` | `/api/products/{id}` | Replace a product | `204` |
| `DELETE` | `/api/products/{id}` | Delete a product | `204` |

## Architecture

```text
HTTP request
    ↓
ProductsController
    ↓
IProductRepository
    ↓
InMemoryProductRepository
    ↓
Dictionary<int, Product>
```

The controller depends on an abstraction rather than the concrete storage implementation.

## Dependency injection

The repository is registered in `Program.cs`:

```csharp
builder.Services.AddSingleton<
    IProductRepository,
    InMemoryProductRepository>();
```

ASP.NET Core supplies it through constructor injection.

A singleton is used so all requests access the same in-memory product collection. The repository uses locking because singleton state may be accessed by concurrent requests.

## Request and response models

`CreateProductRequest` and `UpdateProductRequest` represent incoming data.

`Product` represents the stored and returned resource.

Separate request models prevent clients from supplying server-controlled properties such as the generated product ID.

## Important HTTP responses

### `201 Created`

A successful POST uses `CreatedAtAction()`. The response includes the created product and a `Location` header pointing to its GET endpoint.

### `204 No Content`

Successful update and delete operations return no response body.

### `400 Bad Request`

Invalid names and non-positive prices are rejected.

### `404 Not Found`

Requests for missing product IDs return a clear error response.

## Build and run

From the sample folder:

```bash
dotnet build
dotnet run
```

Use the exact address displayed in the terminal.

Data is held only in memory and resets when the application restarts.

## Example requests

### Get all

```powershell
Invoke-RestMethod http://localhost:<port>/api/products
```

### Create

```powershell
$body = @{
    name = "USB-C Hub"
    price = 2499.00
} | ConvertTo-Json

Invoke-RestMethod `
    -Method Post `
    -Uri http://localhost:<port>/api/products `
    -ContentType "application/json" `
    -Body $body
```

### Update

```powershell
$body = @{
    name = "USB-C Hub Pro"
    price = 2999.00
} | ConvertTo-Json

Invoke-RestMethod `
    -Method Put `
    -Uri http://localhost:<port>/api/products/3 `
    -ContentType "application/json" `
    -Body $body
```

### Delete

```powershell
Invoke-RestMethod `
    -Method Delete `
    -Uri http://localhost:<port>/api/products/3
```

## Test cases

| Scenario | Expected status |
|---|---:|
| Get seeded product | `200` |
| Get missing product | `404` |
| Create valid product | `201` |
| Blank product name | `400` |
| Zero or negative price | `400` |
| Update existing product | `204` |
| Update missing product | `404` |
| Delete existing product | `204` |
| Delete missing product | `404` |

## Student practice

1. Add product category and stock quantity.
2. Add filtering and sorting query parameters.
3. Add pagination.
4. Implement partial updates using PATCH.
5. Replace manual validation with validation attributes.
6. Replace in-memory storage with Entity Framework Core.
7. Add automated controller and repository tests.
8. Add OpenAPI documentation.

## Trainer discussion prompts

- Why does the controller depend on an interface?
- Why are request models separate from `Product`?
- Why does POST return `201`?
- What information does the `Location` header provide?
- Why does a singleton repository require synchronization?
- What limitations does in-memory persistence have?
- When should PUT and PATCH be used?
