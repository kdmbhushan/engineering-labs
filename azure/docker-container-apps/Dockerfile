# Multi-Stage Hardened Container Build for .NET 8 / 9
# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY DockerContainerApps.csproj .
RUN dotnet restore -r linux-musl-x64

COPY . .
RUN dotnet publish DockerContainerApps.csproj \
    -c Release \
    -r linux-musl-x64 \
    --no-restore \
    -o /app/publish \
    /p:PublishReadyToRun=true

# Stage 2: Minimal Distroless / Chiseled Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

# Run as non-root unprivileged app user (security hardening)
USER $APP_UID

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "DockerContainerApps.dll"]
