FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7134

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Application/Application.csproj", "./Application/"]
RUN dotnet restore "Application/Application.csproj" 

COPY . .
WORKDIR "/src/Application"

ARG configuration 
RUN echo ${configuration}
RUN dotnet build "Application.csproj" -c ${configuration} -o /app/build -r linux-x64 --self-contained false

FROM build AS publish
RUN dotnet publish "Application.csproj" -c ${configuration} -o /app/publish -r linux-x64 --self-contained false 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Application.dll"]
