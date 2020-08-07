FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS builder
WORKDIR /app

# Copy files over and restore
COPY . .
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=builder /app/out .
ENV ASPNETCORE_ENVIRONMENT Production
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Skimur.Web.dll