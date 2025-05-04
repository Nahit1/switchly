using MediatR;
using Switchly.Application.Common.Models;
using Switchly.Domain.Entities;
using Switchly.Persistence.Db;

namespace Switchly.Application.Features.Organizations.Commands.CreateOrganization;

public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, ApiResponse<Guid>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateOrganizationCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Organizations.AddAsync(organization, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(organization.Id);
    }
}