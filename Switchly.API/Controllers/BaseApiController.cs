using MediatR;
using Microsoft.AspNetCore.Mvc;
using Switchly.Application.Common.Models;

namespace Switchly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    
    protected IActionResult Success<T>(T data, int statusCode = 200)
    {
        return StatusCode(statusCode, ApiResponse<T>.Ok(data));
    }

    protected IActionResult Error<T>(string message, int statusCode = 400)
    {
        return StatusCode(statusCode, ApiResponse<T>.Fail(message));
    }

    protected IActionResult Error<T>(List<string> messages, int statusCode = 400)
    {
        return StatusCode(statusCode, ApiResponse<T>.Fail(messages));
    }
}