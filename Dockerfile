FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StaffingPortalBackend.csproj", "./"]
RUN dotnet restore "./StaffingPortalBackend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "StaffingPortalBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StaffingPortalBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StaffingPortalBackend.dll"]