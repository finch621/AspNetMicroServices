FROM mcr.microsoft.com/dotnet/aspnet:7.0 as base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /src
COPY ["Services/Basket/Basket.API/Basket.API.csproj", "Services/Basket/Basket.API/"]
RUN dotnet restore "Services/Basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/Services/Basket/Basket.API"
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Basket.API.dll" ]
