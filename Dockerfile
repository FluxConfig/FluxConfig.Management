FROM --platform=${BUILDPLATFORM} mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src

COPY ["src/FluxConfig.Management.Domain/FluxConfig.Management.Domain.csproj", "FluxConfig.Management.Domain/"]
COPY ["src/FluxConfig.Management.Infrastructure/FluxConfig.Management.Infrastructure.csproj", "FluxConfig.Management.Infrastructure/"]
COPY ["src/FluxConfig.Management.Api/FluxConfig.Management.Api.csproj", "FluxConfig.Management.Api/"]

RUN dotnet restore "FluxConfig.Management.Api/FluxConfig.Management.Api.csproj" --arch ${TARGETARCH}
COPY src/. .
RUN dotnet publish "FluxConfig.Management.Api/FluxConfig.Management.Api.csproj" --arch ${TARGETARCH} -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "FluxConfig.Management.Api.dll"]