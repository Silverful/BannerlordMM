#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

EXPOSE 5001

ENV ASPNETCORE_URLS=https://+:5001
ENV ASPNETCORE_ENVIRONMENT Development

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BL.API.WebHost/BL.API.WebHost.csproj", "BL.API.WebHost/"]
COPY ["BL.API.Services/BL.API.Services.csproj", "BL.API.Services/"]
COPY ["BL.API.Core/BL.API.Core.csproj", "BL.API.Core/"]
COPY ["BL.API.DataAccess/BL.API.DataAccess.csproj", "BL.API.DataAccess/"]
RUN dotnet restore "BL.API.WebHost/BL.API.WebHost.csproj"
COPY . .
WORKDIR "/src/BL.API.WebHost"
RUN dotnet build "BL.API.WebHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BL.API.WebHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BL.API.WebHost.dll"]