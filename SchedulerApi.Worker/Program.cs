using Microsoft.EntityFrameworkCore;
using SchedulerApi.Application.Recipient.Contracts;
using SchedulerApi.Application.Recipient.Services;
using SchedulerApi.Application.Schedule.Contracts;
using SchedulerApi.Application.Schedule.Services;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Application.Template.Contracts;
using SchedulerApi.Application.Template.Services;
using SchedulerApi.Application.User.Contracts;
using SchedulerApi.Application.User.Services;
using SchedulerApi.Application.UserIntegration.Contracts;
using SchedulerApi.Application.UserIntegration.Services;
using SchedulerApi.Application.Workers;
using SchedulerApi.Domain.Recipient.Contracts;
using SchedulerApi.Domain.Schedule.Contracts;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.Template.Contracts;
using SchedulerApi.Domain.User.Contracts;
using SchedulerApi.Domain.UserIntegration.Contracts;
using SchedulerApi.Infrastructure.Accessors;
using SchedulerApi.Infrastructure.Persistence;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Interceptors;
using SchedulerApi.Infrastructure.Repositories;
using SchedulerApi.Infrastructure.Services;
using SchedulerApi.Worker;

try
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddDbContext<SchedulerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"))
        .AddInterceptors(new SqliteForeignKeyEnablerInterceptor())
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine));
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<MappingContextAccessor>();
    builder.Services.AddScoped<ICurrentUserAccessor, WorkerCurrentUserAccessor>();
    builder.Services.AddScoped<ITenantProvider, TenantProvider>();
    builder.Services.AddScoped<IApiKeyGenerator, ApiKeyGenerator>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
    builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
    builder.Services.AddScoped<IUserUsageService, UserUsageService>();
    builder.Services.AddScoped<IUpdateUserService, UpdateUserService>();
    builder.Services.AddScoped<IRecipientRepository, RecipientRepository>();
    builder.Services.AddScoped<ICreateRecipientService, CreateRecipientService>();
    builder.Services.AddScoped<IUpdateRecipientService, UpdateRecipientService>();
    builder.Services.AddScoped<IDeleteRecipientService, DeleteRecipientService>();
    builder.Services.AddScoped<IGetRecipientService, GetRecipientService>();
    builder.Services.AddScoped<IUserIntegrationRepository, UserIntegrationRepository>();
    builder.Services.AddScoped<ICreateUserIntegrationService, CreateUserIntegrationService>();
    builder.Services.AddScoped<IUpdateUserIntegrationService, UpdateUserIntegrationService>();
    builder.Services.AddScoped<IDeleteUserIntegrationService, DeleteUserIntegrationService>();
    builder.Services.AddScoped<IGetUserIntegrationService, GetUserIntegrationService>();
    builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
    builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
    builder.Services.AddScoped<ICreateTemplateService, CreateTemplateService>();
    builder.Services.AddScoped<IUpdateTemplateService, UpdateTemplateService>();
    builder.Services.AddScoped<IDeleteTemplateService, DeleteTemplateService>();
    builder.Services.AddScoped<ICreateScheduleService, CreateScheduleService>();
    builder.Services.AddScoped<IUpdateScheduleService, UpdateScheduleService>();
    builder.Services.AddScoped<IDeleteScheduleService, DeleteScheduleService>();
    builder.Services.AddScoped<IGetScheduleService, GetScheduleService>();
    builder.Services.AddScoped<IGetTemplateService, GetTemplateService>();
    builder.Services.AddScoped<ISchedulerIntegrationRunner, SchedulerIntegrationRunner>();
    
    builder.Services.AddHostedService<SchedulerWorker>();
    
    var host = builder.Build();
    
    Console.WriteLine("Built successfully..4");
    
    host.Run();
} catch (Exception ex)
{
    Console.WriteLine($"An error occurred while starting the worker: {ex.Message}");
    throw;
}
