# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# cache için csproj ve sln
COPY Switchly.sln ./
COPY Switchly.API/Switchly.API.csproj Switchly.API/
COPY Switchly.Application/Switchly.Application.csproj Switchly.Application/
COPY Switchly.Persistence/Switchly.Persistence.csproj Switchly.Persistence/
COPY Switchly.Domain/Switchly.Domain.csproj Switchly.Domain/
COPY Switchly.Infrastructure/Switchly.Infrastructure.csproj Switchly.Infrastructure/
COPY Switchly.Shared/Switchly.Shared.csproj Switchly.Shared/

RUN dotnet restore Switchly.sln

# tüm kaynakları kopyala ve publish et
COPY . .
WORKDIR /src/Switchly.API
RUN dotnet publish -c Release -o /app --no-restore

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
