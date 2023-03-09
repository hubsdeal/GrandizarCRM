#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SoftGrid.Web.Host/SoftGrid.Web.Host.csproj", "src/SoftGrid.Web.Host/"]
COPY ["src/SoftGrid.Web.Core/SoftGrid.Web.Core.csproj", "src/SoftGrid.Web.Core/"]
COPY ["src/SoftGrid.Application/SoftGrid.Application.csproj", "src/SoftGrid.Application/"]
COPY ["src/SoftGrid.Application.Shared/SoftGrid.Application.Shared.csproj", "src/SoftGrid.Application.Shared/"]
COPY ["src/SoftGrid.Core.Shared/SoftGrid.Core.Shared.csproj", "src/SoftGrid.Core.Shared/"]
COPY ["src/SoftGrid.Core/SoftGrid.Core.csproj", "src/SoftGrid.Core/"]
COPY ["src/SoftGrid.EntityFrameworkCore/SoftGrid.EntityFrameworkCore.csproj", "src/SoftGrid.EntityFrameworkCore/"]
COPY ["src/SoftGrid.GraphQL/SoftGrid.GraphQL.csproj", "src/SoftGrid.GraphQL/"]
RUN dotnet restore "src/SoftGrid.Web.Host/SoftGrid.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/SoftGrid.Web.Host"
RUN dotnet build "SoftGrid.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SoftGrid.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftGrid.Web.Host.dll"]
