FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY API/API.csproj API/
WORKDIR /src/API
RUN dotnet restore API.csproj


RUN dotnet publish "API/API.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "API.dll"]