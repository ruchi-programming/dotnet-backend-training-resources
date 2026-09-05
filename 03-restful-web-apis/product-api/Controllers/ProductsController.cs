using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Repositories;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<Product>> GetAll()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Product> GetById(int id)
    {
        Product? product = _repository.GetById(id);

        if (product is null)
        {
            return NotFound(
                new
                {
                    Error = $"Product {id} was not found."
                });
        }

        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> Create(
        CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(
                new
                {
                    Error = "Product name is required."
                });
        }

        if (request.Price <= 0)
        {
            return BadRequest(
                new
                {
                    Error = "Price must be greater than zero."
                });
        }

        Product product = _repository.Add(
            request.Name.Trim(),
            request.Price);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(
        int id,
        UpdateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(
                new
                {
                    Error = "Product name is required."
                });
        }

        if (request.Price <= 0)
        {
            return BadRequest(
                new
                {
                    Error = "Price must be greater than zero."
                });
        }

        bool updated = _repository.Update(
            id,
            request.Name.Trim(),
            request.Price);

        if (!updated)
        {
            return NotFound(
                new
                {
                    Error = $"Product {id} was not found."
                });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!_repository.Delete(id))
        {
            return NotFound(
                new
                {
                    Error = $"Product {id} was not found."
                });
        }

        return NoContent();
    }
}
