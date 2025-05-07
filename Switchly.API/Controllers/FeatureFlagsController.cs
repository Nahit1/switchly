using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Switchly.Api.Controllers;
using Switchly.Application.FeatureFlags.Commands.ArchiveFeatureFlag;
using Switchly.Application.FeatureFlags.Commands.CreateFeatureFlag;
using Switchly.Application.FeatureFlags.Commands.UpdateFeatureFlag;
using Switchly.Application.FeatureFlags.Queries;

namespace Switchly.API.Controllers
{
    public class FeatureFlagsController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateFeatureFlag([FromBody] CreateFeatureFlagCommand command)
        {
            var id = await Mediator.Send(command);
            return Success(new { id }, 201); // dönüş tipi: ApiResponse<{ id }>
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Örnek placeholder response
            return Success(new { id });
        }

        [HttpGet("get-all")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllByOrganization()
        {
            var result = await Mediator.Send(new GetAllFeatureFlagsQuery());
            return Success(result.Data!); // ApiResponse wrapper'ı içinde zaten
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Archive(Guid id)
        {
            var result = await Mediator.Send(new ArchiveFeatureFlagCommand(id));

            if (!result.Success)
                return Error<bool>(result.Error!, 404);

            return Success(result.Data!);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFeatureFlagCommand command)
        {
            if (id != command.Id)
                return Error<bool>("ID uyuşmuyor.", 400);

            var result = await Mediator.Send(command);

            if (!result.Success)
                return Error<bool>(result.Error!, 404);

            return Success(result.Data!);
        }

    }

}
