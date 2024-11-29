# Start with the .NET Core SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory to the project directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY BackEnd/*.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY BackEnd/. ./

# Build the application
RUN dotnet build -c Release -o out SportingStatsBackEnd.csproj

# Specify the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the environment variable to Development or Production based on your needs
ARG ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}

# Set the working directory for the runtime image
WORKDIR /app

# Copy the built application from the build image
COPY --from=build /app/out ./

# Copy SSL certificates from the EC2 instance
COPY /certbot/certs/sportingstats.ca/fullchain1.pem /https/ssl/certs/cert.pem
COPY /certbot/certs/sportingstats.ca/privkey1.pem /https/ssl/private/key.pem

COPY BackEnd/appsettings.json ./
COPY BackEnd/appsettings.Development.json ./
COPY BackEnd/appsettings.Production.json ./

#Expose the required ports
EXPOSE 5000 5001

# Set environment variables for Kestrel to use SSL
ENV ASPNETCORE_URLS=https://+:5001
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/ssl/certs/cert.pem
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/https/ssl/private/key.pem

# Specify the entry point
ENTRYPOINT ["dotnet", "SportingStatsBackEnd.dll"]
