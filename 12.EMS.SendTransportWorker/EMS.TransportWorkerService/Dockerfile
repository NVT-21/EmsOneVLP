# Build & chạy worker trên Linux (amd64). Oracle: dùng connection string tới host DB mà container truy cập được.
FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
WORKDIR /src

COPY ["EMS.TransportWorkerService.csproj", "./"]
RUN dotnet restore "EMS.TransportWorkerService.csproj"

COPY . .
RUN dotnet publish "EMS.TransportWorkerService.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0-bookworm-slim AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "EMS.TransportWorkerService.dll"]
