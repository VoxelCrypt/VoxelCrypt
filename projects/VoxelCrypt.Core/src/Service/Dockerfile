FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY projects/VoxelCrypt.Core/src/Kernel/Kernel.csproj projects/VoxelCrypt.Core/src/Kernel/
COPY projects/VoxelCrypt.Core/src/Service/Service.csproj projects/VoxelCrypt.Core/src/Service/
RUN dotnet restore projects/VoxelCrypt.Core/src/Service/Service.csproj

COPY . .
RUN dotnet publish projects/VoxelCrypt.Core/src/Service/Service.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Service.dll"]
