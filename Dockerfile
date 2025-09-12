# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Sadece gereken projeleri kopyala (worker yok)
COPY Switchly.API/Switchly.API.csproj Switchly.API/
COPY Switchly.Application/Switchly.Application.csproj Switchly.Application/
COPY Switchly.Persistence/Switchly.Persistence.csproj Switchly.Persistence/
COPY Switchly.Domain/Switchly.Domain.csproj Switchly.Domain/
COPY Switchly.Infrastructure/Switchly.Infrastructure.csproj Switchly.Infrastructure/
COPY Switchly.Shared/Switchly.Shared.csproj Switchly.Shared/

# (İsteğe bağlı ama yayın ortamlarında faydalı)
ENV DOTNET_CLI_HOME=/tmp
ENV NUGET_XMLDOC_MODE=skip

# 1) Restore (ayrı, daha net log)
RUN dotnet restore Switchly.API/Switchly.API.csproj -v minimal

# Kaynakların tamamını kopyala
COPY . .

# 2) Build (ayrı, publish'ten önce derle)
RUN dotnet build Switchly.API/Switchly.API.csproj -c Release --no-restore -v minimal

# 3) Publish (daha az bellek tüketen bayraklarla)
# - PublishReadyToRun=false : R2R kapalı (RAM ve süreyi düşürür)
# - PublishSingleFile=false : tek dosya paketleme yok (RAM tüketimini düşürür)
# - UseAppHost=false        : native host üretme
RUN dotnet publish Switchly.API/Switchly.API.csproj -c Release --no-restore -o /app \
    -p:PublishReadyToRun=false -p:PublishSingleFile=false -p:UseAppHost=false -v minimal

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
