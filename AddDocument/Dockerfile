FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AddDocument/AddDocument.csproj", "AddDocument/"]
RUN dotnet restore "AddDocument/AddDocument.csproj"
COPY . .
WORKDIR "/src/AddDocument"
RUN dotnet build "AddDocument.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AddDocument.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AddDocument.dll"]
