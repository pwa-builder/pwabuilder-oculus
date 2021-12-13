# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["Microsoft.PWABuilder.Oculus/Microsoft.PWABuilder.Oculus.csproj", "Microsoft.PWABuilder.Oculus_build/"]
RUN dotnet restore "Microsoft.PWABuilder.Oculus_build/Microsoft.PWABuilder.Oculus.csproj"

# Copy everything else and build
WORKDIR "/app/Microsoft.PWABuilder.Oculus_build"
COPY . .
RUN dotnet build "Microsoft.PWABuilder.Oculus.csproj" -c Release -o "/app/build" --no-restore
RUN dotnet publish "Microsoft.PWABuilder.Oculus.csproj" -c Release -o "/app/publish"

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime-env
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Microsoft.PWABuilder.Oculus.dll"]

RUN echo "Unzipping Android Build Tools"
RUN unzip /app/publish.zip


# Bring in the Android dev tools image
# FROM pwabuilder.azurecr.io/pwa-android-build-box:latest as android-base
# COPY --from=runtime-env /app/publish .

