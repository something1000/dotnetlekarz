FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["dotnetlekarz/dotnetlekarz.csproj", "dotnetlekarz/"]
RUN dotnet restore "dotnetlekarz/dotnetlekarz.csproj"
COPY . .
WORKDIR "/src/dotnetlekarz"
RUN dotnet build "dotnetlekarz.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnetlekarz.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY "dotnetlekarz/docvisit.db" .
ENTRYPOINT ["dotnet", "dotnetlekarz.dll"]
