using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Switchly.Api.Controllers;
using Switchly.Application.Features.SegmentRules.Commands.AddSegmentRule;
using Switchly.Application.Features.SegmentRules.Commands.DeleteSegmentRule;
using Switchly.Application.Features.SegmentRules.Dtos;
using Switchly.Application.Features.SegmentRules.Queries.GetSegmentExpressionTree;
using Switchly.Application.Features.SegmentRules.Queries.GetSegmentRulesByFeatureId;

namespace Switchly.Api.Controllers;



public class SegmentRulesController : BaseApiController
{
    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
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

    [HttpGet("{featureFlagId}/segment-tree")]
    public async Task<IActionResult> GetSegmentTree(Guid featureFlagId)
    {
      var result = await Mediator.Send(new GetSegmentExpressionTreeQuery(featureFlagId));
      return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost("{featureFlagId}/segment-tree")]
    public async Task<IActionResult> CreateSegmentTree(Guid featureFlagId, [FromBody] SegmentExpressionDto root)
    {
      var rootId = await Mediator.Send(new CreateSegmentExpressionTreeCommand
      {
          FeatureFlagId = featureFlagId,
          Root = root
      });

      return CreatedAtAction(nameof(GetSegmentTree), new { featureFlagId }, new { rootId });
    }


}
