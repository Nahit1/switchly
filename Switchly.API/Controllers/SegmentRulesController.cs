using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Switchly.Api.Controllers;
using Switchly.Application.Features.SegmentRules.Commands.AddSegmentRule;
using Switchly.Application.Features.SegmentRules.Commands.DeleteSegmentRule;
using Switchly.Application.Features.SegmentRules.Dtos;
using Switchly.Application.Features.SegmentRules.Queries.GetSegmentRulesByFeatureId;

namespace Switchly.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/segment-rules")]
public class SegmentRulesController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> AddRule([FromBody] AddSegmentRuleCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success
            ? Success(result.Data!, 201)
            : Error<Guid>(result.Error!);
    }
    
    [HttpGet("{featureFlagId}")]
    public async Task<IActionResult> GetByFeatureFlagId(Guid featureFlagId)
    {
        var result = await Mediator.Send(new GetSegmentRulesByFeatureIdQuery(featureFlagId));
        return result.Success
            ? Success(result.Data!)
            : Error<List<SegmentRuleDto>>(result.Error!);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteSegmentRuleCommand(id));

        return result.Success
            ? Success(result.Data!)
            : Error<bool>(result.Error!);
    }

}