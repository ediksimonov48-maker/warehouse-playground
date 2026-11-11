FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Playground.csproj .
RUN dotnet restore Playground.csproj

COPY . .
RUN dotnet publish Playground.csproj -c Release -o /app/publish --no-restore
RUN mkdir -p /app/publish/wwwroot/_framework \
	&& find /root/.nuget/packages/microsoft.aspnetcore.app.internal.assets -path '*/_framework/blazor.web.js' -type f -exec cp {} /app/publish/wwwroot/_framework/blazor.web.js \;

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Playground.dll"]
