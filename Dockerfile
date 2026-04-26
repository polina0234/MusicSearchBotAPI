FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY MusicSearchBotAPI.csproj .
RUN dotnet restore "MusicSearchBotAPI.csproj"
COPY . .
RUN dotnet publish "MusicSearchBotAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "MusicSearchBotAPI.dll"]
