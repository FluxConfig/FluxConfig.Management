FROM --platform=${BUILDPLATFORM} mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM --platform=${BUILDPLATFORM} mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src

COPY ["src/FluxConfig.Management.Domain/FluxConfig.Management.Domain.csproj", "FluxConfig.Management.Domain/"]
COPY ["src/FluxConfig.Management.Infrastructure/FluxConfig.Management.Infrastructure.csproj", "FluxConfig.Management.Infrastructure/"]
COPY ["src/FluxConfig.Management.Api/FluxConfig.Management.Api.csproj", "FluxConfig.Management.Api/"]

RUN dotnet restore "FluxConfig.Management.Api/FluxConfig.Management.Api.csproj" --arch ${TARGETARCH}
COPY src/. .
RUN dotnet build "FluxConfig.Management.Api/FluxConfig.Management.Api.csproj" -c Release -o /app/build

WORKDIR "/src/FluxConfig.Management.Api"
FROM build AS publish
RUN dotnet publish "FluxConfig.Management.Api.csproj" --arch ${TARGETARCH}  -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxConfig.Management.Api.dll"]