FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY VOCENuOrderApp/VOCENuOrderApp.csproj VOCENuOrderApp/
RUN dotnet restore VOCENuOrderApp/VOCENuOrderApp.csproj
COPY VOCENuOrderApp/ VOCENuOrderApp/
WORKDIR /src/VOCENuOrderApp
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "VOCENuOrderApp.dll"]
