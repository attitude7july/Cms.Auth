#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Cms.Auth.IdentityProvider/Cms.Auth.IdentityProvider.csproj", "Cms.Auth.IdentityProvider/"]
RUN dotnet restore "Cms.Auth.IdentityProvider/Cms.Auth.IdentityProvider.csproj"
COPY . .
WORKDIR "/src/Cms.Auth.IdentityProvider"
RUN dotnet build "Cms.Auth.IdentityProvider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cms.Auth.IdentityProvider.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Cms.Auth.IdentityProvider.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Cms.Auth.IdentityProvider.dll