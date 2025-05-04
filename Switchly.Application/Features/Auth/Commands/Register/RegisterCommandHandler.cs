using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Models;
using Switchly.Application.Features.Auth.Dtos;
using Switchly.Application.Features.Auth.Interfaces;
using Switchly.Domain.Entities;
using Switchly.Persistence.Db;
using System.Security.Cryptography;
using System.Text;

namespace Switchly.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse<LoginResultDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RegisterCommandHandler(ApplicationDbContext dbContext, IJwtTokenGenerator tokenGenerator)
    {
        _dbContext = dbContext;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ApiResponse<LoginResultDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (exists)
            return ApiResponse<LoginResultDto>.Fail("Bu e-posta zaten kullanılıyor.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = Hash(request.Password),
            Role = request.Role,
            OrganizationId = request.OrganizationId,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = _tokenGenerator.GenerateToken(user.Id, user.OrganizationId, user.Role);

        return ApiResponse<LoginResultDto>.Ok(new LoginResultDto
        {
            UserId = user.Id,
            OrganizationId = user.OrganizationId,
            Role = user.Role,
            Token = token
        });
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}
