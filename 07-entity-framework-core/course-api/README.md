# Entity Framework Core Course API

## Learning objectives

After completing this example, students will be able to:

- Configure EF Core with SQLite.
- Define an entity and database context.
- Configure a model using the Fluent API.
- Register a context with dependency injection.
- Perform asynchronous CRUD operations.
- Use tracking and no-tracking queries appropriately.
- Seed demonstration data.
- Pass cancellation tokens to database operations.

## Architecture

```text
HTTP request
    ↓
Course endpoints
    ↓
CourseDbContext
    ↓
EF Core SQLite provider
    ↓
courses.db
```

## Entity configuration

`CourseDbContext` configures:

- Table name
- Primary key
- Required title
- Maximum title length
- Fee precision
- Seed data

The Fluent API keeps persistence configuration centralized instead of placing every database concern on the entity class.

## Database registration

```csharp
builder.Services.AddDbContext<CourseDbContext>(
    options => options.UseSqlite(connectionString));
```

`AddDbContext` registers the context as scoped by default. A context instance is normally used for one request and should not be shared concurrently.

## Endpoints

| Method | Route | Purpose |
|---|---|---|
| `GET` | `/api/courses` | Get all courses |
| `GET` | `/api/courses/{id}` | Get one course |
| `POST` | `/api/courses` | Create a course |
| `PUT` | `/api/courses/{id}` | Update a course |
| `DELETE` | `/api/courses/{id}` | Delete a course |

## Tracking behaviour

Read-only queries use:

```csharp
AsNoTracking()
```

This avoids change-tracking overhead when returned entities will not be modified.

Update and delete operations load tracked entities. Changes are detected and persisted by:

```csharp
SaveChangesAsync()
```

## Asynchronous operations

Database calls use asynchronous EF Core methods such as:

```csharp
ToListAsync()
SingleOrDefaultAsync()
FindAsync()
SaveChangesAsync()
```

Request cancellation tokens are passed through so abandoned requests can cancel pending database work when supported.

## Database initialization

The example uses:

```csharp
EnsureCreatedAsync()
```

This creates a database directly from the current model and is convenient for a small, self-contained lesson.

It is not a replacement for migrations in an evolving production application. Do not mix `EnsureCreated`-based schema management with a normal migrations workflow for the same database.

## Build and run

From the sample folder:

```bash
dotnet restore
dotnet build
dotnet run
```

The first run creates:

```text
courses.db
```

The database file is excluded by `.gitignore`.

To reset the demonstration data:

1. Stop the application.
2. Delete the local `courses.db` file.
3. Run the application again.

This deletion is appropriate only for the disposable demonstration database.

## Example create request

```json
{
  "title": "ASP.NET Core Web API",
  "durationHours": 40,
  "fee": 18000
}
```

## Expected status codes

| Scenario | Status |
|---|---:|
| Get courses | `200` |
| Get existing course | `200` |
| Get missing course | `404` |
| Create valid course | `201` |
| Create invalid course | `400` |
| Update existing course | `204` |
| Delete existing course | `204` |

## Student practice

1. Add category and difficulty properties.
2. Add filtering, sorting and pagination.
3. Add a unique title constraint.
4. Add a related `Module` entity.
5. Compare eager, explicit and lazy loading.
6. Replace `EnsureCreated` with migrations.
7. Add optimistic concurrency handling.
8. Write integration tests using a separate test database.

## Trainer discussion prompts

- What responsibility does `DbContext` have?
- Why is it normally scoped?
- When should `AsNoTracking()` be used?
- What does `SaveChangesAsync()` do?
- Why use request DTOs?
- How is `EnsureCreated` different from migrations?
- Why should database files and connection secrets not be committed?
