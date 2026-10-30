FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["SimpleECommerceAPI/SimpleECommerceAPI.csproj", "SimpleECommerceAPI/"]
RUN dotnet restore "SimpleECommerceAPI/SimpleECommerceAPI.csproj"
COPY . .
WORKDIR "/src/SimpleECommerceAPI"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SimpleECommerceAPI.dll"]