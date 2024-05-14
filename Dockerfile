# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["customer-api/customer-api.csproj", "customer-api/"]
RUN dotnet restore "customer-api/customer-api.csproj"
COPY . .
WORKDIR "/src/customer-api"
RUN dotnet build "customer-api.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "customer-api.csproj" -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "customer-api.dll"]
