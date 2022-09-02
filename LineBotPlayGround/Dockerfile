#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LineBotPlayGround/LineBotPlayGround.csproj", "LineBotPlayGround/"]
RUN dotnet restore "LineBotPlayGround/LineBotPlayGround.csproj"
COPY . .
WORKDIR "/src/LineBotPlayGround"
RUN dotnet build "LineBotPlayGround.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LineBotPlayGround.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LineBotPlayGround.dll"]