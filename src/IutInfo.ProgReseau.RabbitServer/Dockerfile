FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
WORKDIR /app
COPY artifacts/server/ ./
ENTRYPOINT ["dotnet", "IutInfo.ProgReseau.RabbitServer.dll"]