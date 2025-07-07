# Stage 1: Build the Blazor application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the Blazor project file and restore dependencies
# This step helps with Docker layer caching
COPY BlazorQuizApp.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Publish the application for release
RUN dotnet publish -c Release -o out

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

# Set the ASP.NET Core URL to listen on all interfaces
ENV ASPNETCORE_URLS=http://+:80

# Expose the port your Blazor app will listen on
EXPOSE 80

# Command to run the application
ENTRYPOINT ["dotnet", "BlazorQuizApp.dll"]