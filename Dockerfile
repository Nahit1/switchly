# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Yalnızca gerekli csproj'lar (worker yok)
COPY Switchly.API/Switchly.API.csproj Switchly.API/
COPY Switchly.Application/Switchly.Application.csproj Switchly.Application/
COPY Switchly.Persistence/Switchly.Persistence.csproj Switchly.Persistence/
COPY Switchly.Domain/Switchly.Domain.csproj Switchly.Domain/
COPY Switchly.Infrastructure/Switchly.Infrastructure.csproj Switchly.Infrastructure/
COPY Switchly.Shared/Switchly.Shared.csproj Switchly.Shared/

# İlk restore (cache'li)
RUN dotnet restore Switchly.API/Switchly.API.csproj

# Tüm kaynaklar
COPY . .
WORKDIR /src/Switchly.API

# Güvenli publish (restore dahil)
RUN dotnet publish -c Release -o /app

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
