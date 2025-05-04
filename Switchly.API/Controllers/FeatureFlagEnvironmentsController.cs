using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Switchly.Api.Controllers;
using Switchly.Application.Features.FeatureFlagEnvironments.Commands.CreateFeatureFlagEnvironment;

namespace Switchly.API.Controllers
{
    
    public class FeatureFlagEnvironmentsController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeatureFlagEnvironmentCommand command)
        {
            var id = await Mediator.Send(command);
            return Success(new { id }, 201);
        }
    }
}
