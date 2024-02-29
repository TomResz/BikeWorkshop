#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BikeWorkshop.API/BikeWorkshop.API.csproj", "BikeWorkshop.API/"]
COPY ["BikeWorkshop.Application/BikeWorkshop.Application.csproj", "BikeWorkshop.Application/"]
COPY ["BikeWorkshop.Domain/BikeWorkshop.Domain.csproj", "BikeWorkshop.Domain/"]
COPY ["BikeWorkshop.Shared/BikeWorkshop.Shared.csproj", "BikeWorkshop.Shared/"]
COPY ["BikeWorkshop.Infrastructure/BikeWorkshop.Infrastructure.csproj", "BikeWorkshop.Infrastructure/"]
RUN dotnet restore "./BikeWorkshop.API/./BikeWorkshop.API.csproj"
COPY . .
WORKDIR "/src/BikeWorkshop.API"
RUN dotnet build "./BikeWorkshop.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BikeWorkshop.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BikeWorkshop.API.dll"]