# Order Processing Domain Model

## Learning objectives

After completing this example, students will be able to:

- Model a small business domain using C# types.
- Use records for value-oriented data.
- Represent state using an enum.
- Protect object state through encapsulation.
- Validate arguments with guard clauses.
- Apply domain rules inside methods.
- Calculate totals using LINQ and `decimal`.

## Domain model

```text
Order
├── Customer name
├── Status
└── Order items
    ├── Product
    ├── Quantity
    └── Line total
```

## Important design choices

### Records

`Product` and `OrderItem` are records because they primarily represent related values.

```csharp
record Product(int Id, string Name, decimal UnitPrice);
```

Records provide value-based equality and concise immutable-style modelling.

### Class

`Order` is a class because it has identity, mutable state and domain behaviour.

### Enum

`OrderStatus` limits status values to a defined set:

```text
Pending
Confirmed
Dispatched
Delivered
Cancelled
```

### Encapsulation

The internal item list is private:

```csharp
private readonly List<OrderItem> _items = new();
```

Consumers receive a read-only view and cannot modify the list directly.

### Guard clauses

Constructors and methods reject invalid arguments immediately. This helps prevent an object from entering an invalid state.

### Domain rules

An order:

- Cannot contain an invalid product.
- Cannot accept a non-positive quantity.
- Cannot be confirmed when empty.
- Can be confirmed only while pending.

### Monetary values

The program uses `decimal`, which is generally preferable to binary floating-point types for monetary calculations.

## Build and run

From the sample folder:

```bash
dotnet build
dotnet run
```

The project targets .NET 8, enables nullable-reference analysis and treats warnings as errors.

## Expected output

The output includes:

```text
Order: 1001
Customer: Aarav Sharma
Status: Confirmed
Items:
- Mechanical Keyboard x 1
- Wireless Mouse x 2
Order total: ...
```

The currency symbol and formatting depend on the computer’s regional settings. The calculated total is `6097.00`.

## Test ideas

| Scenario | Expected result |
|---|---|
| Valid products and quantities | Order total is calculated |
| Empty customer name | `ArgumentException` |
| Zero or negative order ID | `ArgumentOutOfRangeException` |
| Quantity zero | `ArgumentOutOfRangeException` |
| Invalid product | `ArgumentException` |
| Confirm empty order | `InvalidOperationException` |
| Confirm twice | `InvalidOperationException` |

## Student practice

1. Add a discount percentage.
2. Add shipping charges.
3. Prevent changes after confirmation.
4. Add methods for dispatch and delivery.
5. Combine repeated products into one item.
6. Create automated tests for every domain rule.
7. Move each type into a separate source file.
8. Persist orders using a repository in a later lesson.

## Trainer discussion prompts

- Why is `Order` a class while the other models are records?
- Why is the list private?
- Why expose a read-only collection?
- Which rules belong inside the domain model?
- Why is `decimal` used for prices?
- What is the difference between argument validation and a domain-state rule?
- How could this model later be exposed through a Web API?
