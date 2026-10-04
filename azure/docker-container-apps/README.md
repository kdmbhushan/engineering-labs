# Docker & Azure Container Apps: Hardened Cloud Deployment

> **Laboratory Experiment**: Multi-stage rootless Docker container builds, minimal distroless/chiseled runtimes, cgroups limits, and Azure Container Apps (ACA) deployment.

🔗 **Canonical Deep Dive**: [Build and Deploy .NET Core Applications Using Docker Containers and Azure](https://bhushankadam.dev/blog/build-and-deploy-net-core-applications-using-docker-containers) on [bhushankadam.dev](https://bhushankadam.dev).

## Key Patterns Demonstrated
1. **Multi-Stage Build**: Isolates full .NET SDK during the compilation stage and outputs binaries to a tiny, attack-surface-minimized alpine runtime image.
2. **Rootless Security**: Executes under unprivileged `USER $APP_UID` to eliminate root-escape container vulnerabilities.
3. **Infrastructure as Code**: Declarative Bicep template (`containerapp.bicep`) for serverless Azure Container Apps environment.
