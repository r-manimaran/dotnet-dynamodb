#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dynamodb-converters/dynamodb-converters.csproj", "dynamodb-converters/"]
RUN dotnet restore "dynamodb-converters/dynamodb-converters.csproj"
COPY . .
WORKDIR "/src/dynamodb-converters"
RUN dotnet build "dynamodb-converters.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dynamodb-converters.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dynamodb-converters.dll"]