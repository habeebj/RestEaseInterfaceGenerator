FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OpenApiParser/OpenApiParser.csproj", "OpenApiParser/"]
RUN dotnet restore "OpenApiParser/OpenApiParser.csproj"
COPY . .
WORKDIR "/src/OpenApiParser"
RUN dotnet build "OpenApiParser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenApiParser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenApiParser.dll"]
