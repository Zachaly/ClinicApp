# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClinicApp.WebApi/ClinicApp.WebApi.csproj", "ClinicApp.WebApi/"]
COPY ["ClinicApp.Application/ClinicApp.Application.csproj", "ClinicApp.Application/"]
COPY ["ClinicApp.Domain/ClinicApp.Domain.csproj", "ClinicApp.Domain/"]
COPY ["ClinicApp.Infrastructure/ClinicApp.Infrastructure.csproj", "ClinicApp.Infrastructure/"]
COPY ["ClinicApp.Database/ClinicApp.Database.csproj", "ClinicApp.Database/"]
RUN dotnet restore "./ClinicApp.WebApi/ClinicApp.WebApi.csproj"
COPY . .
WORKDIR "/src/ClinicApp.WebApi"
RUN dotnet build "./ClinicApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClinicApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicApp.WebApi.dll"]