FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

ENV SRC_ROOT_DIR="projects/VoxelCrypt/src"
ENV TARGET_PROJECT="projects/VoxelCrypt/src/VoxelCrypt.Core/Service/Service.csproj"

COPY global.json global.json
COPY ${SRC_ROOT_DIR}/Directory.Build.props ${SRC_ROOT_DIR}/Directory.Build.props
COPY ${SRC_ROOT_DIR}/Directory.Packages.props ${SRC_ROOT_DIR}/Directory.Packages.props

COPY ${SRC_ROOT_DIR}/VoxelCrypt.Core/Kernel/Kernel.csproj ${SRC_ROOT_DIR}/VoxelCrypt.Core/Kernel/
COPY ${SRC_ROOT_DIR}/VoxelCrypt.Core/Service/Service.csproj ${SRC_ROOT_DIR}/VoxelCrypt.Core/Service/

RUN dotnet restore ${TARGET_PROJECT}

COPY . .
RUN dotnet publish ${TARGET_PROJECT} -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Service.dll"]
