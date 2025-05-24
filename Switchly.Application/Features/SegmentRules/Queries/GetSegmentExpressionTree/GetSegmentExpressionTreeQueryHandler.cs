using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Features.SegmentRules.Queries.GetSegmentExpressionTree;
using Switchly.Domain.Entities;
using Switchly.Persistence.Db;

public class GetSegmentExpressionTreeQueryHandler : IRequestHandler<GetSegmentExpressionTreeQuery, SegmentExpressionDto?>
{
  private readonly ApplicationDbContext _context;

  public GetSegmentExpressionTreeQueryHandler(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<SegmentExpressionDto?> Handle(GetSegmentExpressionTreeQuery request, CancellationToken cancellationToken)
  {
    var rootExpression = await _context.SegmentExpressions
      .Where(x => x.FeatureFlagId == request.FeatureFlagId && x.ParentExpressionId == null)
      .Select(x => x.Id)
      .FirstOrDefaultAsync(cancellationToken);

    if (rootExpression == Guid.Empty)
      return null;

    var expressionTree = await LoadExpressionTreeAsync(rootExpression, cancellationToken);

    return MapToDtoRecursive(expressionTree);

  }

  private SegmentExpressionDto MapToDtoRecursive(SegmentExpression entity)
  {
    return new SegmentExpressionDto
    {
      Id = entity.Id,
      Operator = entity.Operator,
      Rule = entity.SegmentRule is not null
        ? new SegmentRuleDto
        {
          Id = entity.SegmentRule.Id,
          Property = entity.SegmentRule.Property,
          Operator = entity.SegmentRule.Operator,
          Value = entity.SegmentRule.Value,
          RolloutPercentage = entity.SegmentRule.RolloutPercentage
        }
        : null,
      Children = entity.Children.Select(MapToDtoRecursive).ToList()
    };
  }

  private async Task<SegmentExpression> LoadExpressionTreeAsync(Guid expressionId, CancellationToken cancellationToken)
  {
    var expression = await _context.SegmentExpressions
      .Include(x => x.SegmentRule)
      .FirstOrDefaultAsync(x => x.Id == expressionId, cancellationToken);

    if (expression == null) return null!;

    var children = await _context.SegmentExpressions
      .Where(x => x.ParentExpressionId == expression.Id)
      .ToListAsync(cancellationToken);

    foreach (var child in children)
    {
      var fullChild = await LoadExpressionTreeAsync(child.Id, cancellationToken);
      expression.Children.Add(fullChild);
    }

    return expression;
  }

}
