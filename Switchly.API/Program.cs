using System.Text;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Switchly.Api.Middlewares;
using Switchly.Application.Common.Interfaces;
using Switchly.Application.Common.Messaging;
using Switchly.Application.FeatureFlags.Commands;
using Switchly.Application.FeatureFlags.Commands.CreateFeatureFlag;
using Switchly.Application.FeatureFlags.Interfaces;
using Switchly.Application.FeatureFlags.Services;
using Switchly.Application.Features.Auth.Interfaces;
using Switchly.Infrastructure.Auth;
using Switchly.Infrastructure.Messaging;
using Switchly.Persistence.Db;
using Switchly.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://0.0.0.0:80");


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();


var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"]!;
var jwtIssuer = jwtSettings["Issuer"]!;
var jwtAudience = jwtSettings["Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateFeatureFlagCommand).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(CreateFeatureFlagCommand).Assembly);

builder.Services.AddScoped<IEvaluateEventPublisher, MassTransitEvaluateEventPublisher>();
builder.Services.AddSingleton<IRedisKeyProvider, RedisKeyProvider>();


builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IFeatureFlagService, FeatureFlagService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IFeatureFlagEvaluator, FeatureFlagEvaluator>();



builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", policy =>
  {
    policy
      .AllowAnyOrigin()  // Geliştirme aşamasında açıyoruz, production'da kısıtlaman gerekebilir
      .AllowAnyHeader()
      .AllowAnyMethod();
  });
});

builder.Services.AddMassTransit(x =>
{
  x.UsingRabbitMq((context, cfg) =>
  {
    cfg.Host("rabbitmq://localhost", h =>
    {
      h.Username("guest");
      h.Password("guest");
    });
  });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

