FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SchedulerApi.Worker/SchedulerApi.Worker.csproj", "SchedulerApi.Worker/"]
RUN dotnet restore "SchedulerApi.Worker/SchedulerApi.Worker.csproj"
COPY . .
WORKDIR "/src/SchedulerApi.Worker"
RUN dotnet build "SchedulerApi.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SchedulerApi.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchedulerApi.Worker.dll"]
