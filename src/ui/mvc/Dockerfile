FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["ui/mvc/mvc.csproj", "./"]
RUN dotnet restore "./mvc.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ui/mvc/mvc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ui/mvc/mvc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "mvc.dll"]
