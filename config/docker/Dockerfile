# This Dockerfile builds and images a .NET 8 application using a multi-stage build process.
# !!!
# It works for the code structure provided in the project. If your structure is different,
# you may need to adjust the paths accordingly. 
# !!!


# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file and project files
COPY Adria.sln ./
COPY src/Adria.Main/Adria.Main.csproj ./src/Adria.Main/
COPY src/Adria.Domain/Adria.Domain.csproj ./src/Adria.Domain/
COPY src/Adria.Application/Adria.Application.csproj ./src/Adria.Application/
COPY src/Adria.Infrastructure/Adria.Infrastructure.csproj ./src/Adria.Infrastructure/
COPY tests/UnitTests/UnitTests.csproj ./tests/UnitTests/

# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY . .

# Build the solution
RUN dotnet build --configuration Release --no-restore

# Publish the main project
RUN dotnet publish src/Adria.Main/Adria.Main.csproj --configuration Release --no-build --output /app/publish

# Use the official .NET 8 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Ensure the SQL scripts are in the right location for the app to find them
RUN mkdir -p /app/Persistence/Scripts
COPY --from=build /app/src/Adria.Infrastructure/Persistence/Scripts/*.sql /app/Persistence/Scripts/

# Expose the port that the application listens on
EXPOSE 8000

# Set the entry point
ENTRYPOINT ["dotnet", "Adria.Main.dll"]