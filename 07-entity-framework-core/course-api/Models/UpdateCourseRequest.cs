namespace CourseApi.Models;

public sealed record UpdateCourseRequest(
    string Title,
    int DurationHours,
    decimal Fee);
