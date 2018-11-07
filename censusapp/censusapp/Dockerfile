FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY censusapp/censusapp.csproj censusapp/
RUN dotnet restore censusapp/censusapp.csproj
COPY . .
WORKDIR /src/censusapp
RUN dotnet build censusapp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish censusapp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "censusapp.dll"]
