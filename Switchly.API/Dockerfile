# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# (CACHE için) sadece csproj'ları kopyala
COPY Switchly.sln ./
COPY Switchly.API/Switchly.API.csproj Switchly.API/
COPY Switchly.Application/Switchly.Application.csproj Switchly.Application/
COPY Switchly.Persistence/Switchly.Persistence.csproj Switchly.Persistence/
COPY Switchly.Domain/Switchly.Domain.csproj Switchly.Domain/
COPY Switchly.Infrastructure/Switchly.Infrastructure.csproj Switchly.Infrastructure/

RUN dotnet restore Switchly.sln

# tüm kaynakları kopyala ve publish et
COPY . .
WORKDIR /src/Switchly.API
RUN dotnet publish -c Release -o /app --no-restore

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
# Sağlık kontrolü için app'te /health endpoint'in olsun
HEALTHCHECK --interval=30s --timeout=3s --start-period=20s CMD curl -fsS http://localhost:8080/health || exit 1
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
