FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Ortam: düşük RAM/IO için sadeleştir
ENV DOTNET_NOLOGO=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_CLI_HOME=/tmp \
    NUGET_XMLDOC_MODE=skip \
    DOTNET_DISABLE_PARALLEL=1 \
    NUGET_PACKAGES=/tmp/nuget

# Tüm kaynakları önce kopyala (props/targets kaçmasın)
COPY . .

# (Teşhis) SDK bilgisi
RUN dotnet --info

# RESTORE (paralel kapalı, başarısız kaynakları yoksay)
RUN dotnet restore Switchly.API/Switchly.API.csproj \
    --disable-parallel --ignore-failed-sources --nologo -v minimal

# BUILD (analizörler kapalı, tek çekirdek)
RUN dotnet build Switchly.API/Switchly.API.csproj -c Release --no-restore \
    -p:RunAnalyzersDuringBuild=false -p:UseSharedCompilation=false -v minimal

# PUBLISH (R2R/SingleFile kapalı → RAM daha az)
RUN dotnet publish Switchly.API/Switchly.API.csproj -c Release --no-restore -o /app \
    -p:PublishReadyToRun=false -p:PublishSingleFile=false -p:UseAppHost=false -p:RunAnalyzersDuringBuild=false -v minimal

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
