# 1. Build-Phase (SDK nutzen, um den Code zu kompilieren)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HouseholdTasksAPI.csproj", "./"]
RUN dotnet restore "HouseholdTasksAPI.csproj"

# Kopiere den restlichen Code und baue die App
COPY . .
RUN dotnet publish "HouseholdTasksAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Laufzeit-Phase (Nur die kleine Runtime nutzen, ohne Compiler)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Erstelle einen Ordner für die SQLite-Datenbank
RUN mkdir -p /app/data

# Ab .NET 8 lauschen Container standardmäßig auf Port 8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HouseholdTasksAPI.dll"]