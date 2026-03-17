FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview5 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview5 AS build
WORKDIR /src
COPY ["MMN.Api/MMN.Api.csproj", "MMN.Api/"]
RUN dotnet restore "MMN.Api/MMN.Api.csproj"
COPY . .
WORKDIR "/src/MMN.Api"
RUN dotnet build "MMN.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MMN.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MMN.Api.dll"]