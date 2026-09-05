using ProductApi.Models;

namespace ProductApi.Repositories;

public sealed class InMemoryProductRepository
    : IProductRepository
{
    private readonly object _syncRoot = new();

    private readonly Dictionary<int, Product> _products =
        new()
        {
            [1] = new Product(
                1,
                "Mechanical Keyboard",
                3499.00m),

            [2] = new Product(
                2,
                "Wireless Mouse",
                1299.00m)
        };

    private int _nextId = 2;

    public IReadOnlyCollection<Product> GetAll()
    {
        lock (_syncRoot)
        {
            return _products.Values
                .OrderBy(product => product.Id)
                .ToArray();
        }
    }

    public Product? GetById(int id)
    {
        lock (_syncRoot)
        {
            return _products.GetValueOrDefault(id);
        }
    }

    public Product Add(string name, decimal price)
    {
        lock (_syncRoot)
        {
            int id = ++_nextId;
            Product product = new(id, name, price);

            _products.Add(id, product);
            return product;
        }
    }

    public bool Update(
        int id,
        string name,
        decimal price)
    {
        lock (_syncRoot)
        {
            if (!_products.ContainsKey(id))
            {
                return false;
            }

            _products[id] = new Product(id, name, price);
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_syncRoot)
        {
            return _products.Remove(id);
        }
    }
}

/*
Important design points:
- The controller will depend on IProductRepository, not the concrete class.
- The repository is registered as a singleton.
- A lock protects the mutable dictionary from concurrent requests.
- Immutable Product records prevent callers from changing stored objects.
- GetAll() returns a separate array rather than exposing the dictionary.
The project still needs its controller before the final build.
*/
