#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SoftGrid.Migrator/SoftGrid.Migrator.csproj", "src/SoftGrid.Migrator/"]
COPY ["src/SoftGrid.EntityFrameworkCore/SoftGrid.EntityFrameworkCore.csproj", "src/SoftGrid.EntityFrameworkCore/"]
COPY ["src/SoftGrid.Core/SoftGrid.Core.csproj", "src/SoftGrid.Core/"]
COPY ["src/SoftGrid.Core.Shared/SoftGrid.Core.Shared.csproj", "src/SoftGrid.Core.Shared/"]
RUN dotnet restore "src/SoftGrid.Migrator/SoftGrid.Migrator.csproj"
COPY . .
WORKDIR "/src/src/SoftGrid.Migrator"
RUN dotnet build "SoftGrid.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SoftGrid.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftGrid.Migrator.dll", "-s"]
