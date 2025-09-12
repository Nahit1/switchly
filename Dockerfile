# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Build ortamını sadeleştir (daha az indirme / RAM)
ENV DOTNET_NOLOGO=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_CLI_HOME=/tmp \
    NUGET_XMLDOC_MODE=skip \
    DOTNET_DISABLE_PARALLEL=1

# Basit ve sağlam: önce tüm kaynakları kopyala (cache daha az ama teşhis garantili)
COPY . .

# SDK bilgisini yazdır (loglarda gözüksün)
RUN dotnet --info

# 1) Restore (full log)
RUN dotnet restore Switchly.API/Switchly.API.csproj -v minimal

# 2) Build (daha düşük RAM, analizörler kapalı, tek core)
RUN dotnet build Switchly.API/Switchly.API.csproj -c Release --no-restore \
    -v minimal /m:1 \
    -p:RunAnalyzersDuringBuild=false \
    -p:UseSharedCompilation=false \
    -p:ContinuousIntegrationBuild=true

# 3) Publish (R2R ve SingleFile kapalı → RAM düşer)
RUN dotnet publish Switchly.API/Switchly.API.csproj -c Release --no-restore -o /app \
    -v minimal \
    -p:PublishReadyToRun=false \
    -p:PublishSingleFile=false \
    -p:UseAppHost=false \
    -p:RunAnalyzersDuringBuild=false

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet","Switchly.API.dll"]
