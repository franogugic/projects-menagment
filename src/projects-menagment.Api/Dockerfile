FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["projects-menagment.sln", "./"]
COPY ["src/projects-menagment.Api/projects-menagment.Api.csproj", "src/projects-menagment.Api/"]
COPY ["src/projects-menagment.Application/projects-menagment.Application.csproj", "src/projects-menagment.Application/"]
COPY ["src/projects-menagment.Domain/projects-menagment.Domain.csproj", "src/projects-menagment.Domain/"]
COPY ["src/projects-menagment.Infrastructure/projects-menagment.Infrastructure.csproj", "src/projects-menagment.Infrastructure/"]
COPY ["tests/projects-menagment.Application.Tests/projects-menagment.Application.Tests.csproj", "tests/projects-menagment.Application.Tests/"]
COPY ["tests/projects-menagment.Api.IntegrationTests/projects-menagment.Api.IntegrationTests.csproj", "tests/projects-menagment.Api.IntegrationTests/"]

RUN dotnet restore "projects-menagment.sln"

COPY . .
RUN dotnet publish "src/projects-menagment.Api/projects-menagment.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "projects-menagment.Api.dll"]
