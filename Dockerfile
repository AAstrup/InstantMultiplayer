# FROM microsoft/aspnetcore:2.0 AS base
FROM mcr.microsoft.com/dotnet/runtime:3.1-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 61001

# FROM microsoft/aspnetcore-build:2.0 AS build
FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR /src
COPY InstantMultiplayer.sln ./
COPY Host/*.csproj ./Host/
COPY TestClient/*.csproj ./TestClient/
COPY UnityIntegration/*.csproj ./UnityIntegration/
COPY Synchronization/*.csproj ./Synchronization/
COPY Communication/*.csproj ./Communication/
COPY UnityIntegrationEditor/*.csproj ./UnityIntegrationEditor/
COPY Synchronization.Test/*.csproj ./Synchronization.Test/
COPY InstantMultiplayerAppService/*.csproj ./InstantMultiplayerAppService/

RUN dotnet restore
COPY . .

WORKDIR /src/Host
RUN dotnet build -c Release -o /app

WORKDIR /src/TestClient
RUN dotnet build -c Release -o /app

WORKDIR /src/UnityIntegration
RUN dotnet build -c Release -o /app

WORKDIR /src/Synchronization
RUN dotnet build -c Release -o /app

WORKDIR /src/Communication
RUN dotnet build -c Release -o /app

WORKDIR /src/Synchronization.Test
RUN dotnet build -c Release -o /app

WORKDIR /src/InstantMultiplayerAppService
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Host.dll"]