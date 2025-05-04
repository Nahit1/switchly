using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Switchly.Api.Controllers;
using Switchly.Application.Features.Auth.Commands.Login;
using Switchly.Application.Features.Auth.Commands.Register;
using Switchly.Application.Features.Auth.Dtos;

namespace Switchly.Api.Controllers;

public class AuthController : BaseApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success
            ? Success(result.Data!)
            : Error<LoginCommand>(result.Error!);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetSecureData()
    {
        var userId = User.FindFirst("sub")?.Value;
        var orgId = User.FindFirst("organizationId")?.Value;
        var role = User.FindFirst("role")?.Value;

        return Success(new
        {
            userId,
            organizationId = orgId,
            role
        });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await Mediator.Send(command);

        if (!result.Success)
            return Error<LoginResultDto>(result.Error!);

        return Success(result.Data!, 201);
    }
}