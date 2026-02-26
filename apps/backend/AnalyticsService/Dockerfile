# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY AnalyticsService.csproj ./
RUN dotnet restore

# Copy the source code and build the application
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 8 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

# Expose the port the app runs on
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "AnalyticsService.dll"]