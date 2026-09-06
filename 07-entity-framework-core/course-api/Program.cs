using CourseApi.Data;
using CourseApi.Endpoints;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

string connectionString =
    builder.Configuration.GetConnectionString(
        "CourseDatabase")
    ?? throw new InvalidOperationException(
        "Course database connection string is missing.");

builder.Services.AddDbContext<CourseDbContext>(
    options => options.UseSqlite(connectionString));

WebApplication app = builder.Build();

await using (AsyncServiceScope scope =
    app.Services.CreateAsyncScope())
{
    CourseDbContext database =
        scope.ServiceProvider
            .GetRequiredService<CourseDbContext>();

    await database.Database.EnsureCreatedAsync();
}

app.MapCourseEndpoints();

await app.RunAsync();
