# Build & chạy worker đồng bộ danh mục EOMS trên Linux (amd64)
FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
WORKDIR /src

COPY ["EMS.EomsCategorySyncWorker.csproj", "./"]
RUN dotnet restore "EMS.EomsCategorySyncWorker.csproj"

COPY . .
RUN dotnet publish "EMS.EomsCategorySyncWorker.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0-bookworm-slim AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "EMS.EomsCategorySyncWorker.dll"]
