FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Switchly.API/Switchly.API.csproj", "Switchly.API/"]
COPY ["Switchly.Persistence/Switchly.Persistence.csproj", "Switchly.Persistence/"]
COPY ["Switchly.Domain/Switchly.Domain.csproj", "Switchly.Domain/"]
RUN dotnet restore "Switchly.API/Switchly.API.csproj"
COPY . .
WORKDIR "/src/Switchly.API"
RUN dotnet build "Switchly.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Switchly.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Switchly.API.dll"]
