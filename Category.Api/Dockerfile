#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Category.Api/Category.Api.csproj", "Category.Api/"]
RUN dotnet restore "Category.Api/Category.Api.csproj"
COPY . .
WORKDIR "/src/Category.Api"
RUN dotnet build "Category.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Category.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Category.Api.dll"]