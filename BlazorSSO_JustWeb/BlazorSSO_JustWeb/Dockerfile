FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY BlazorSSO_JustWeb.csproj .
RUN dotnet restore BlazorSSO_JustWeb.csproj
COPY . .
RUN dotnet build BlazorSSO_JustWeb.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish BlazorSSO_JustWeb.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf