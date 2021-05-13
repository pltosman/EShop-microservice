FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/EShopMicroService/EShop.Products/Products.EShop.Products.csproj", "src/EShopMicroService/EShop.Products/"]
RUN dotnet restore "src/EShopMicroService/EShop.Products/Products.EShop.Products.csproj"
COPY . .
WORKDIR "/src/src/EShopMicroService/EShop.Products"
RUN dotnet build "Products.EShop.Products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Products.EShop.Products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.EShop.Products.dll"]
