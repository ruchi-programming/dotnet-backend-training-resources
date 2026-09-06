using Microsoft.AspNetCore.Mvc;
using StudentRegistrationApi.Models;

namespace StudentRegistrationApi.Controllers;

[ApiController]
[Route("api/registrations")]
public sealed class RegistrationsController
    : ControllerBase
{
    [HttpPost]
    public ActionResult<StudentRegistrationResponse> Create(
        StudentRegistrationRequest request)
    {
        StudentRegistrationResponse response = new(
            Guid.NewGuid(),
            request.FullName.Trim(),
            request.Email.Trim(),
            request.Course,
            DateTimeOffset.UtcNow);

        return Created(
            $"/api/registrations/{response.RegistrationId}",
            response);
    }
}
