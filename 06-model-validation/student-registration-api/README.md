# Student Registration API: Model Validation

## Learning objectives

After completing this example, students will be able to:

- Define a dedicated API request model.
- Apply data-annotation validation attributes.
- Use automatic validation with `[ApiController]`.
- Interpret validation error responses.
- Distinguish input validation from domain rules.
- Test valid, missing and out-of-range values.

## Endpoint

| Method | Route | Purpose | Success status |
|---|---|---|---:|
| `POST` | `/api/registrations` | Validate and accept a registration | `201` |

The sample creates a response but does not persist registration data.

## Validation rules

| Property | Rules |
|---|---|
| `FullName` | Required; 2–80 characters |
| `Email` | Required; valid email format |
| `Age` | 16–100 |
| `Course` | `C`, `C++`, `C#` or `DSA` |
| `ExperienceLevel` | 1–5 |

## Data annotations

The request model uses attributes from:

```csharp
System.ComponentModel.DataAnnotations
```

Examples include:

```csharp
[Required]
[StringLength(80, MinimumLength = 2)]
[EmailAddress]
[Range(16, 100)]
[RegularExpression(...)]
```

These attributes describe common input constraints close to the request properties.

## Automatic validation

The controller includes:

```csharp
[ApiController]
```

When model validation fails, ASP.NET Core normally returns `400 Bad Request` before the controller action executes.

The response identifies invalid properties and their associated messages.

## Valid request

```json
{
  "fullName": "Ananya Singh",
  "email": "ananya@example.com",
  "age": 22,
  "course": "C++",
  "experienceLevel": 2
}
```

## Example invalid request

```json
{
  "fullName": "",
  "email": "incorrect-email",
  "age": 14,
  "course": "Python",
  "experienceLevel": 8
}
```

Expected result:

```text
400 Bad Request
```

## Build and run

From the sample folder:

```bash
dotnet build
dotnet run
```

Use the exact local address shown in the terminal.

## Test cases

| Scenario | Expected result |
|---|---|
| All fields valid | `201 Created` |
| Missing full name | `400 Bad Request` |
| One-character name | `400 Bad Request` |
| Invalid email | `400 Bad Request` |
| Age below 16 | `400 Bad Request` |
| Unsupported course | `400 Bad Request` |
| Experience level above 5 | `400 Bad Request` |
| Malformed JSON | `400 Bad Request` |

The course regular expression is case-sensitive, so `c` does not match `C`.

## Input validation versus domain validation

Input validation checks whether a request is structurally acceptable.

Examples:

- Required fields
- Length limits
- Numeric ranges
- Basic format rules

Domain validation enforces business behaviour.

Examples:

- A course has available seats.
- A student is not already registered.
- Registration is still open.
- Prerequisites have been completed.

Domain rules normally belong in application or domain services rather than only in request attributes.

## Why the response has no Location header

The example does not store registrations or provide a GET-by-ID endpoint. It therefore returns `201 Created` without advertising a resource URL that clients cannot retrieve.

A persistent version should add storage and a GET endpoint, then use `CreatedAtAction()`.

## Student practice

1. Add a phone-number field.
2. Add a preferred-session option.
3. Create a custom validation attribute.
4. Validate two related properties together.
5. Customize the validation response format.
6. Add persistence and a GET-by-ID endpoint.
7. Write integration tests for every validation rule.
8. Move business validation into a registration service.

## Trainer discussion prompts

- What does `[ApiController]` contribute?
- When does automatic validation occur?
- Why use a request DTO instead of a domain entity?
- What are the limitations of regular-expression validation?
- How is input validation different from a business rule?
- When should `CreatedAtAction()` be used?
