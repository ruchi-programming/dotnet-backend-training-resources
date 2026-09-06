namespace CourseApi.Models;

public sealed record CreateCourseRequest(
    string Title,
    int DurationHours,
    decimal Fee);
