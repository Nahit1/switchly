using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Switchly.Api.Controllers;

namespace Switchly.API.Controllers
{
    public class EvaluateController : BaseApiController
    {
        private readonly IFeatureFlagEvaluator _evaluator;

        public EvaluateController(IFeatureFlagEvaluator evaluator)
        {
            _evaluator = evaluator;
        }

        [HttpPost]
        public async Task<IActionResult> Evaluate([FromBody] EvaluateFeatureFlagRequest request)
        {
            var isEnabled = await _evaluator.IsEnabledAsync(request.FlagKey, request.UserContextModel);
            return Success(new { enabled = isEnabled });
        }
    }

}
