# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj de todos los proyectos necesarios
COPY Nubelity.API/*.csproj Nubelity.API/
COPY Nubelity.Application/*.csproj Nubelity.Application/
COPY Nubelity.Domain/*.csproj Nubelity.Domain/
COPY Nubelity.Infrastructure/*.csproj Nubelity.Infrastructure/

# Restaurar dependencias SOLO del proyecto principal
RUN dotnet restore Nubelity.API/Nubelity.API.csproj

# Copiar todo el código
COPY . .

# Publicar
RUN dotnet publish Nubelity.API/Nubelity.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Nubelity.API.dll"]
