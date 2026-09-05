using System;
using System.Collections.Generic;
using System.Linq;

internal enum OrderStatus
{
    Pending,
    Confirmed,
    Dispatched,
    Delivered,
    Cancelled
}

internal sealed record Product(
    int Id,
    string Name,
    decimal UnitPrice);

internal sealed record OrderItem(
    Product Product,
    int Quantity)
{
    public decimal LineTotal =>
        Product.UnitPrice * Quantity;
}

internal sealed class Order
{
    private readonly List<OrderItem> _items = new();

    public Order(int id, string customerName)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(id),
                "Order ID must be positive.");
        }

        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException(
                "Customer name is required.",
                nameof(customerName));
        }

        Id = id;
        CustomerName = customerName;
    }

    public int Id { get; }

    public string CustomerName { get; }

    public OrderStatus Status { get; private set; } =
        OrderStatus.Pending;

    public IReadOnlyCollection<OrderItem> Items =>
        _items.AsReadOnly();

    public void AddItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product.Id <= 0 ||
            string.IsNullOrWhiteSpace(product.Name) ||
            product.UnitPrice < 0)
        {
            throw new ArgumentException(
                "Product details are invalid.",
                nameof(product));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be positive.");
        }

        _items.Add(new OrderItem(product, quantity));
    }

    public decimal CalculateTotal()
    {
        return _items.Sum(item => item.LineTotal);
    }

    public void Confirm()
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException(
                "An empty order cannot be confirmed.");
        }

        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only a pending order can be confirmed.");
        }

        Status = OrderStatus.Confirmed;
    }
}

internal static class Program
{
    private static void Main()
    {
        Product keyboard =
            new(1, "Mechanical Keyboard", 3499.00m);

        Product mouse =
            new(2, "Wireless Mouse", 1299.00m);

        Order order = new(1001, "Aarav Sharma");

        order.AddItem(keyboard, 1);
        order.AddItem(mouse, 2);
        order.Confirm();

        Console.WriteLine($"Order: {order.Id}");
        Console.WriteLine($"Customer: {order.CustomerName}");
        Console.WriteLine($"Status: {order.Status}");
        Console.WriteLine("Items:");

        foreach (OrderItem item in order.Items)
        {
            Console.WriteLine(
                $"- {item.Product.Name} x {item.Quantity}" +
                $" = {item.LineTotal:C}");
        }

        Console.WriteLine(
            $"Order total: {order.CalculateTotal():C}");
    }
}

/*

This sample demonstrates:
•	Records for value-oriented models
•	An enum for state
•	Encapsulation
•	Read-only collection exposure
•	Guard clauses
•	Domain rules
•	LINQ aggregation
•	decimal for monetary calculations

*/
