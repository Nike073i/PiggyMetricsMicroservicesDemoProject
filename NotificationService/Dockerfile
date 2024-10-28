FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

COPY . /app/
RUN dotnet restore ./NotificationService/NotificationService.csproj
RUN dotnet publish ./NotificationService/NotificationService.csproj -c Release -o out 

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "NotificationService.dll"]