# Use the official .NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official .NET Core SDK as a parent image for building
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["pergisafar-web/pergisafar-web.csproj", "pergisafar-web/"]
RUN dotnet restore "pergisafar-web/pergisafar-web.csproj"
COPY . .
WORKDIR "/src/pergisafar-web"
RUN dotnet build "pergisafar-web.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "pergisafar-web.csproj" -c Release -o /app/publish

# Set the entry point for the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pergisafar-web.dll"]