# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0.100 AS build
WORKDIR /src

# Hafif ve deterministik build için bazı env'ler
ENV DOTNET_NOLOGO=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_CLI_HOME=/tmp \
    NUGET_XMLDOC_MODE=skip \
    DOTNET_DISABLE_PARALLEL=1 \
    NUGET_PACKAGES=/tmp/nuget

# Tüm repo içeriklerini kopyala (props/targets kaçmasın)
COPY . .

# SDK bilgisini logla (teşhis için)
RUN dotnet --info

# 1) Restore
RUN dotnet restore Switchly.API/Switchly.API.csproj \
    --disable-parallel --ignore-failed-sources \
    --source https://api.nuget.org/v3/index.json -v minimal

# 2) Build
RUN dotnet build Switchly.API/Switchly.API.csproj -c Release --no-restore \
    -p:RunAnalyzersDuringBuild=false -p:UseSharedCompilation=false -v minimal

# 3) Publish
RUN dotnet publish Switchly.API/Switchly.API.csproj -c Release --no-restore -o /app \
    -p:PublishReadyToRun=false -p:PublishSingleFile=false -p:UseAppHost=false \
    -p:RunAnalyzersDuringBuild=false -v minimal

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
