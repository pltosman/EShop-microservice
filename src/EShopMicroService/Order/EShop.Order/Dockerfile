#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Order/EShop.Order/EShop.Order.csproj", "Order/EShop.Order/"]
COPY ["Order/Ordering.Application/Ordering.Application.csproj", "Order/Ordering.Application/"]
COPY ["Order/Ordering.Domain/Ordering.Domain.csproj", "Order/Ordering.Domain/"]
COPY ["Order/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Order/Ordering.Infrastructure/"]
RUN dotnet restore "Order/EShop.Order/EShop.Order.csproj"
COPY . .
WORKDIR "/src/Order/EShop.Order"
RUN dotnet build "EShop.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShop.Order.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.Order.dll"]
