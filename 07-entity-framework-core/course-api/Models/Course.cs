namespace CourseApi.Models;

public sealed class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int DurationHours { get; set; }

    public decimal Fee { get; set; }
}
