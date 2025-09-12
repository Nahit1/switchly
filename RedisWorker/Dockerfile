# ----- build -----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# (Bu iki satır sende yoksa KALDIRILDI)
# COPY Directory.Packages.props ./
# COPY NuGet.config ./
# COPY global.json ./

# Worker ve referansları
COPY RedisWorker/RedisWorker.csproj RedisWorker/
COPY Switchly.Application/Switchly.Application.csproj Switchly.Application/
COPY Switchly.Persistence/Switchly.Persistence.csproj Switchly.Persistence/
COPY Switchly.Domain/Switchly.Domain.csproj Switchly.Domain/
COPY Switchly.Infrastructure/Switchly.Infrastructure.csproj Switchly.Infrastructure/
COPY Switchly.Shared/Switchly.Shared.csproj Switchly.Shared/

RUN dotnet restore RedisWorker/RedisWorker.csproj

# tüm kaynakları kopyala ve publish
COPY . .
WORKDIR /src/RedisWorker
RUN dotnet publish -c Release -o /app

# ----- runtime -----
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet","RedisWorker.dll"]
