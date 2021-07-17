# FROM microsoft/aspnetcore:2.0 AS base
FROM mcr.microsoft.com/dotnet/runtime:3.1-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 61001

# FROM microsoft/aspnetcore-build:2.0 AS build
FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR /src
COPY InstantMultiplayer.sln ./
COPY SharedMessages/*.csproj ./SharedMessages/
COPY InstantMultiplayerServer/*.csproj ./InstantMultiplayerServer/
COPY InstantMultiplayerTestClient/*.csproj ./InstantMultiplayerTestClient/
COPY UnityIntegration/*.csproj ./UnityIntegration/

RUN dotnet restore
COPY . .
WORKDIR /src/SharedMessages
RUN dotnet build -c Release -o /app

WORKDIR /src/InstantMultiplayerServer
RUN dotnet build -c Release -o /app

WORKDIR /src/InstantMultiplayerTestClient
RUN dotnet build -c Release -o /app

WORKDIR /src/UnityIntegration
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "InstantMultiplayerServer.dll"]