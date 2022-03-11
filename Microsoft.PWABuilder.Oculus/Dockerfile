#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
EXPOSE 80
RUN  apt-get update  -y \
 && apt-get install unzip \
 && apt install -y android-sdk \
 && apt-get -y install default-jre-headless 
#
COPY . .
RUN unzip -j "./Resources/ovr-platform-util-pwa.zip" -d "/app/publish"
RUN dotnet restore "Microsoft.PWABuilder.Oculus.csproj"
RUN dotnet build "Microsoft.PWABuilder.Oculus.csproj" -c Release -o "/app/build" --no-restore
RUN dotnet publish "Microsoft.PWABuilder.Oculus.csproj" -c Release -o "/app/publish/"



RUN chmod -R 777 /app/
#
WORKDIR ./publish
ENTRYPOINT ["dotnet", "MicrosoftPWABuilderOculus.dll"]

















## Copy tools over 
#COPY Resources/ovr-platform-util-pwa.zip .
#RUN unzip "ovr-platform-util-pwa.zip" 
#RUN apt update && apt install -y android-sdk 
#WORKDIR ./ovr-platform-util-pwa	
#RUN ./ovr-platform-util create-pwa -a ./apktool -o output.apk --web-manifest-url https://webboard.app/manifest.json --android-sdk /path/to/sdk/  --package-name app.webboard
#
##
##

# syntax=docker/dockerfile:1
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
#RUN  apt-get update  -y \
 #&& apt-get install unzip 
#COPY "./Resources/ovr-platform-util-pwa.zip" .
#RUN unzip "./ovr-platform-util-pwa.zip" -d .
#RUN chmod -R 777 ./ovr-platform-util-pwa/ 
##RUN chmod -R 777 tmp/
#RUN ./ovr-platform-util-pwa/ovr-platform-util create-pwa -a ./apktool -o output.apk --web-manifest-url https://webboard.app/manifest.json --android-sdk /path/to/sdk/  --package-name app.webboard
#
