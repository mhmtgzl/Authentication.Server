#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Common.Auth.Api/Common.Auth.Api.csproj", "Common.Auth.Api/"]
RUN dotnet restore "Common.Auth.Api/Common.Auth.Api.csproj"
COPY . .
WORKDIR "/src/Common.Auth.Api"
RUN dotnet build "Common.Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Common.Auth.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Common.Auth.Api.dll"]