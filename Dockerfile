# ----- build -----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Stabil & düşük RAM
ENV DOTNET_NOLOGO=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_CLI_HOME=/tmp \
    NUGET_XMLDOC_MODE=skip \
    DOTNET_DISABLE_PARALLEL=1 \
    NUGET_PACKAGES=/tmp/nuget

# Tüm repo içeriklerini al (props/targets kaçmasın)
COPY . .

# Hızlı dosya/kasa kontrolleri (logda göreceğiz)
RUN echo "=== PWD ===" && pwd && \
    echo "=== Tree (first 3 levels) ===" && find . -maxdepth 3 -type f | sort && \
    echo "=== SDK INFO ===" && dotnet --info && \
    echo "=== Check csproj path ===" && test -f Switchly.API/Switchly.API.csproj && echo "OK: Switchly.API/Switchly.API.csproj exists"

# NuGet kaynaklarını sadece nuget.org ile sınırla (diagnostic log aç)
RUN echo "=== DOTNET RESTORE (diag) ===" && \
    dotnet restore Switchly.API/Switchly.API.csproj \
      --disable-parallel \
      --ignore-failed-sources \
      --source https://api.nuget.org/v3/index.json \
      -v diag

# Eğer restore geçerse build/publish (minimal log + düşük RAM bayrakları)
RUN dotnet build Switchly.API/Switchly.API.csproj -c Release --no-restore \
      -p:RunAnalyzersDuringBuild=false -p:UseSharedCompilation=false -v minimal
RUN dotnet publish Switchly.API/Switchly.API.csproj -c Release --no-restore -o /app \
      -p:PublishReadyToRun=false -p:PublishSingleFile=false -p:UseAppHost=false -p:RunAnalyzersDuringBuild=false -v minimal

# ----- runtime -----
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
