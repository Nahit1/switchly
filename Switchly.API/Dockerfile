# 1. Base image (ASP.NET Core .NET 9 runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# 2. Build image (SDK ile proje build edilir)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Tüm projeyi al (kök dizindeki tüm klasörler dahil)
COPY . .

# ✅ .csproj yolu düzeltildi
RUN dotnet publish "Switchly.API/Switchly.API.csproj" -c Release -o /app/publish

# 3. Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Switchly.API.dll"]
