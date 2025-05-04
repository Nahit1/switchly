using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Switchly.Api.Controllers;
using Switchly.Application.Features.Organizations.Commands.CreateOrganization;

namespace Switchly.API.Controllers
{
    
    
    public class OrganizationsController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationCommand command)
        {
            var result = await Mediator.Send(command);
            return result.Success
                ? Success(result.Data!, 201)
                : Error<Guid>(result.Error!);
        }
    }
}
