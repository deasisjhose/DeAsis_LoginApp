# Use an official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DeAsis_LoginApp/DeAsis_LoginApp.csproj", "DeAsis_LoginApp/"]
RUN dotnet restore "DeAsis_LoginApp/DeAsis_LoginApp.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/DeAsis_LoginApp"
RUN dotnet build "DeAsis_LoginApp.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "DeAsis_LoginApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create a final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeAsis_LoginApp.dll"]
