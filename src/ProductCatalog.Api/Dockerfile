FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Packages.props ./
COPY src/ProductCatalog.Api/ProductCatalog.Api.csproj src/ProductCatalog.Api/

RUN dotnet restore src/ProductCatalog.Api/ProductCatalog.Api.csproj

COPY src/ src/

RUN dotnet publish src/ProductCatalog.Api/ProductCatalog.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "ProductCatalog.Api.dll"]
