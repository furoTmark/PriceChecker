FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PriceChecker/PriceChecker.csproj", "PriceChecker/"]
RUN dotnet restore "PriceChecker/PriceChecker.csproj"
COPY . .
WORKDIR "/src/PriceChecker"
RUN dotnet build "PriceChecker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PriceChecker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceChecker.dll"]
