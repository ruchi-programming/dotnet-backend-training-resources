using CourseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data;

public sealed class CourseDbContext : DbContext
{
    public CourseDbContext(
        DbContextOptions<CourseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses =>
        Set<Course>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(
            entity =>
            {
                entity.ToTable("Courses");

                entity.HasKey(course => course.Id);

                entity.Property(course => course.Title)
                    .HasMaxLength(120)
                    .IsRequired();

                entity.Property(course => course.Fee)
                    .HasPrecision(10, 2);

                entity.HasData(
                    new Course
                    {
                        Id = 1,
                        Title = "C Programming",
                        DurationHours = 60,
                        Fee = 12000.00m
                    },
                    new Course
                    {
                        Id = 2,
                        Title = "Data Structures",
                        DurationHours = 50,
                        Fee = 15000.00m
                    });
            });
    }
}
