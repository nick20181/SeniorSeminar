FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 27017

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Campus.Service.Address.csproj", ""]
RUN dotnet restore "./Campus.Service.Address.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Campus.Service.Address.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Campus.Service.Address.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Campus.Service.Address.dll"]