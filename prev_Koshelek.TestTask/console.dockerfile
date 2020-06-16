# Dockerfile

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 50000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Common/Koshelek.TestTask.Domain/Koshelek.TestTask.Domain.csproj", "Common/Koshelek.TestTask.Domain/"]
COPY ["Tests/ConnectionSamples/ConnectionSamples.csproj", "Tests/ConnectionSamples/"]
RUN dotnet restore "Tests/ConnectionSamples/ConnectionSamples.csproj"
COPY . .
WORKDIR "/src/Tests/ConnectionSamples"
RUN dotnet build "ConnectionSamples.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConnectionSamples.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConnectionSamples.dll"]