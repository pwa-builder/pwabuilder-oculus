#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.




FROM pwabuilder.azurecr.io/pwa-oculus-build-box:latest as base
WORKDIR /app
COPY "./Microsoft.PWABuilder.Oculus.csproj" "Microsoft.PWABuilder.Oculus_build/"
RUN dotnet restore "Microsoft.PWABuilder.Oculus_build/Microsoft.PWABuilder.Oculus.csproj"

EXPOSE 80
EXPOSE 443
#
#
WORKDIR "/app/Microsoft.PWABuilder.Oculus_build"
COPY . .
RUN dotnet build "Microsoft.PWABuilder.Oculus.csproj" -c Release -o "/app/build" --no-restore
RUN dotnet publish "Microsoft.PWABuilder.Oculus.csproj" -c Release -o "/app/publish"

#COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
ENV JAVA_HOME /usr/lib/jvm/java-8-openjdk-amd64/bin as publish
FROM mcr.microsoft.com/dotnet/aspnet:6.0
RUN echo java -version
WORKDIR /app/publish
COPY --from=base /app/publish ./
COPY --from=base /usr/lib/jvm/java-8-openjdk-amd64 ./jdk
ENTRYPOINT "dotnet" "MicrosoftPWABuilderOculus.dll"

