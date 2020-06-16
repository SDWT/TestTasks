#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["API/Koshelek.TestTask.Api/Koshelek.TestTask.Api.csproj", "API/Koshelek.TestTask.Api/"]
RUN dotnet restore "API/Koshelek.TestTask.Api/Koshelek.TestTask.Api.csproj"
COPY . .
WORKDIR "/src/API/Koshelek.TestTask.Api"
RUN dotnet build "Koshelek.TestTask.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Koshelek.TestTask.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Koshelek.TestTask.Api.dll"]
