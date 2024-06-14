# Start with the .NET Core SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory to the project directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./

# Build the application
RUN dotnet build -c Release -o out SportingStatsBackEnd.csproj

# Specify the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory for the runtime image
WORKDIR /app

# Copy the built application from the build image
COPY --from=build /app/out ./

# Specify the entry point
ENTRYPOINT ["dotnet", "SportingStatsBackEnd.dll"]