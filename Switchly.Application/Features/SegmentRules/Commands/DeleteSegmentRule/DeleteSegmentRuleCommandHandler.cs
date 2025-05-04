using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Models;
using Switchly.Persistence.Db;

namespace Switchly.Application.Features.SegmentRules.Commands.DeleteSegmentRule;

public class DeleteSegmentRuleCommandHandler : IRequestHandler<DeleteSegmentRuleCommand, ApiResponse<bool>>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteSegmentRuleCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteSegmentRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = await _dbContext.SegmentRules
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (rule is null)
            return ApiResponse<bool>.Fail("Segment kuralı bulunamadı.");

        _dbContext.SegmentRules.Remove(rule);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true);
    }
}