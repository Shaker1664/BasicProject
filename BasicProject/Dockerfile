
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["BasicProject/BasicProject.csproj", "BasicProject/"]
COPY ["BasicProject.Services/BasicProject.Services.csproj", "BasicProject.Services/"]
COPY ["BasicProject.Entity/BasicProject.Entity.csproj", "BasicProject.Entity/"]
COPY ["BasicProject.Persistence/BasicProject.Persistence.csproj", "BasicProject.Persistence/"]
RUN dotnet restore "BasicProject/BasicProject.csproj"
COPY . .
WORKDIR "/src/BasicProject"
RUN dotnet build "BasicProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasicProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasicProject.dll"]