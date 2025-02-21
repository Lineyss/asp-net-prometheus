FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Server.API/Server.API.csproj", "Server.API/"]
COPY ["Server.Application/Server.Application.csproj", "Server.Application/"]
COPY ["Server.DataAccess/Server.DataAccess.csproj", "Server.DataAccess/"]
COPY ["Server.Domain/Server.Domain.csproj", "Server.Domain/"]
RUN dotnet restore "Server.API/Server.API.csproj"
COPY . .
WORKDIR "/src/Server.API"
RUN dotnet build "Server.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Server.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.API.dll"]
