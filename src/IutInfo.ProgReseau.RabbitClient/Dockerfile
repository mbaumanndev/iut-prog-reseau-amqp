FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
WORKDIR /app
COPY artifacts/client/ ./
ENTRYPOINT ["dotnet", "IutInfo.ProgReseau.RabbitClient.dll"]