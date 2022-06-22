FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Pjfm.WebClient/Pjfm.WebClient.csproj", "Pjfm.WebClient/"]
RUN dotnet restore "src/Pjfm.WebClient/Pjfm.WebClient.csproj"
COPY . .
WORKDIR "/src/Pjfm.WebClient"
RUN dotnet build "Pjfm.WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pjfm.WebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pjfm.WebClient.dll"]
