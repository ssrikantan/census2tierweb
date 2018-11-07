FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY censusapi/censusapi.csproj censusapi/
RUN dotnet restore censusapi/censusapi.csproj
COPY . .
WORKDIR /src/censusapi
RUN dotnet build censusapi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish censusapi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "censusapi.dll"]
