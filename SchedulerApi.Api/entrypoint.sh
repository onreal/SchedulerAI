rm -f /src/App_Data/*.db
dotnet ef database update --project /src/SchedulerApi.Infrastructure --startup-project /src/SchedulerApi.Api
exec dotnet watch --project /src/SchedulerApi.Api run