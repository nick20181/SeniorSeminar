FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["DockerTestTakeTwo/DockerTestTakeTwo.csproj", "DockerTestTakeTwo/"]
RUN dotnet restore "DockerTestTakeTwo/DockerTestTakeTwo.csproj"
COPY . .
WORKDIR "/src/DockerTestTakeTwo"
RUN dotnet build "DockerTestTakeTwo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DockerTestTakeTwo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DockerTestTakeTwo.dll"]
