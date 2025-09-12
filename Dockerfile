FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# (ÖNEMLİ) Central Package Mgmt ve nuget config dosyalarını önce kopyala (varsa)
COPY Directory.Packages.props ./           # varsa
COPY NuGet.config ./                        # varsa
COPY global.json ./                         # varsa

# Yalnızca gerekli csproj'lar
COPY Switchly.API/Switchly.API.csproj Switchly.API/
COPY Switchly.Application/Switchly.Application.csproj Switchly.Application/
COPY Switchly.Persistence/Switchly.Persistence.csproj Switchly.Persistence/
COPY Switchly.Domain/Switchly.Domain.csproj Switchly.Domain/
COPY Switchly.Infrastructure/Switchly.Infrastructure.csproj Switchly.Infrastructure/
COPY Switchly.Shared/Switchly.Shared.csproj Switchly.Shared/

# İlk restore (cache'li)
RUN dotnet restore Switchly.API/Switchly.API.csproj

# Kaynakların tamamını kopyala
COPY . .
WORKDIR /src/Switchly.API

# (Güvenli) publish: no-restore kullanma ya da son bir restore yap
# Seçenek 1: tek komutta güvenli publish
RUN dotnet publish -c Release -o /app
# Seçenek 2 (alternatif):
# RUN dotnet restore
# RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
