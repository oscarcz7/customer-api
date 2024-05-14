FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["customer-api/customer-api.csproj", "customer-api/"]
RUN dotnet restore "customer-api/customer-api.csproj"
COPY . .
WORKDIR "/src/customer-api"
RUN dotnet build "customer-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "customer-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "customer-api.dll"]
