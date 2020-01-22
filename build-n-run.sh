#!/bin/bash

dotnet restore
dotnet build -c Release --no-restore
dotnet publish src/IutInfo.ProgReseau.RabbitClient -c Release -o artifacts/client --no-build
dotnet publish src/IutInfo.ProgReseau.RabbitServer -c Release -o artifacts/server --no-build
docker-compose up --build --force-recreate
