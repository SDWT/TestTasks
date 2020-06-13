#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Clients/Koshelek.TestTask.Client/Koshelek.TestTask.Client.csproj", "Clients/Koshelek.TestTask.Client/"]
RUN dotnet restore "Clients/Koshelek.TestTask.Client/Koshelek.TestTask.Client.csproj"
COPY . .
WORKDIR "/src/Clients/Koshelek.TestTask.Client"
RUN dotnet build "Koshelek.TestTask.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Koshelek.TestTask.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Koshelek.TestTask.Client.dll"]