#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Products/EShop.Products/EShop.Products.csproj", "Products/EShop.Products/"]
RUN dotnet restore "Products/EShop.Products/EShop.Products.csproj"
COPY . .
WORKDIR "/src/Products/EShop.Products"
RUN dotnet build "EShop.Products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShop.Products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.Products.dll"]
