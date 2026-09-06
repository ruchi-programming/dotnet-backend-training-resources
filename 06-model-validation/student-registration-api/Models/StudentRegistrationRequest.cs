using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationApi.Models;

public sealed class StudentRegistrationRequest
{
    [Required]
    [StringLength(
        80,
        MinimumLength = 2)]
    public string FullName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Range(16, 100)]
    public int Age { get; init; }

    [Required]
    [RegularExpression(
        "^(C|C\\+\\+|C#|DSA)$",
        ErrorMessage =
            "Course must be C, C++, C# or DSA.")]
    public string Course { get; init; } = string.Empty;

    [Range(1, 5)]
    public int ExperienceLevel { get; init; }
}
