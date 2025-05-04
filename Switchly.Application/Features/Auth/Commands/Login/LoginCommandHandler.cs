using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Models;
using Switchly.Application.Features.Auth.Dtos;
using Switchly.Application.Features.Auth.Interfaces;
using Switchly.Persistence.Db;
using System.Security.Cryptography;
using System.Text;

namespace Switchly.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResultDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginCommandHandler(ApplicationDbContext dbContext, IJwtTokenGenerator tokenGenerator)
    {
        _dbContext = dbContext;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ApiResponse<LoginResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(u => u.Organization)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null || user.PasswordHash != Hash(request.Password))
            return ApiResponse<LoginResultDto>.Fail("Email ya da şifre hatalı.");

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