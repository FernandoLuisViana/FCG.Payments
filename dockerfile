# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos .csproj e restaura
COPY ["FCG.Payments.API/FCG.Payments.API.csproj", "FCG.Payments.API/"]
COPY ["FCG.Payments.Domain/FCG.Payments.Domain.csproj", "FCG.Payments.Domain/"]
COPY ["FCG.Payments.Infra/FCG.Payments.Infra.csproj", "FCG.Payments.Infra/"]
COPY ["FCG.Payments.Services/FCG.Payments.Services.csproj", "FCG.Payments.Services/"]
RUN dotnet restore "FCG.Payments.API/FCG.Payments.API.csproj"

# Copia o código e build
COPY . .
WORKDIR "/src/FCG.Payments.API"
RUN dotnet build "FCG.Payments.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "FCG.Payments.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "FCG.Payments.API.dll"]