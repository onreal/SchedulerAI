using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SchedulerApi.Application.Agents.ContactEnricher.Services;
using SchedulerApi.Application.Agents.Contracts;
using SchedulerApi.Application.Agents.IntentClassifier.Services;
using SchedulerApi.Application.Agents.ScheduleParser.Services;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Recipient.Contracts;
using SchedulerApi.Application.Recipient.Services;
using SchedulerApi.Application.Reminder.Contract;
using SchedulerApi.Application.Reminder.Services;
using SchedulerApi.Application.Schedule.Contracts;
using SchedulerApi.Application.Schedule.Services;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Application.Tasks.Contracts;
using SchedulerApi.Application.Tasks.Services;
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
using SchedulerApi.GenerativeApi.Middlewares;
using SchedulerApi.Infrastructure.Accessors;
using SchedulerApi.Infrastructure.Persistence;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Interceptors;
using SchedulerApi.Infrastructure.Repositories;
using SchedulerApi.Infrastructure.Services;
using SchedulerApi.Integrations.Generative.ChatGpt.Services;
using SchedulerApi.Integrations.Transactional.Twilio.Services;
using GlobalExceptionMiddleware = SchedulerApi.GenerativeApi.Middlewares.GlobalExceptionMiddleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddOpenApi();
services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddDbContext<SchedulerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"))
        .AddInterceptors(new SqliteForeignKeyEnablerInterceptor())
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine));
services.AddHttpContextAccessor();

services.AddScoped<MappingContextAccessor>();
services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
services.AddScoped<ITenantProvider, TenantProvider>();
services.AddScoped<IApiKeyGenerator, ApiKeyGenerator>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUnitOfWork, EfUnitOfWork>();
services.AddScoped<IRegisterUserService, RegisterUserService>();
services.AddScoped<IUserUsageService, UserUsageService>();
services.AddScoped<IUpdateUserService, UpdateUserService>();
services.AddScoped<IRecipientRepository, RecipientRepository>();
services.AddScoped<ICreateRecipientService, CreateRecipientService>();
services.AddScoped<IUpdateRecipientService, UpdateRecipientService>();
services.AddScoped<IDeleteRecipientService, DeleteRecipientService>();
services.AddScoped<IGetRecipientService, GetRecipientService>();
services.AddScoped<IUserIntegrationRepository, UserIntegrationRepository>();
services.AddScoped<ICreateUserIntegrationService, CreateUserIntegrationService>();
services.AddScoped<IUpdateUserIntegrationService, UpdateUserIntegrationService>();
services.AddScoped<IDeleteUserIntegrationService, DeleteUserIntegrationService>();
services.AddScoped<IGetUserIntegrationService, GetUserIntegrationService>();
services.AddScoped<ITemplateRepository, TemplateRepository>();
services.AddScoped<IScheduleRepository, ScheduleRepository>();
services.AddScoped<ICreateTemplateService, CreateTemplateService>();
services.AddScoped<IUpdateTemplateService, UpdateTemplateService>();
services.AddScoped<IDeleteTemplateService, DeleteTemplateService>();
services.AddScoped<ICreateScheduleService, CreateScheduleService>();
services.AddScoped<IUpdateScheduleService, UpdateScheduleService>();
services.AddScoped<IDeleteScheduleService, DeleteScheduleService>();
services.AddScoped<IGetScheduleService, GetScheduleService>();
services.AddScoped<IGetTemplateService, GetTemplateService>();
services.AddScoped<ITaskService, TaskService>();
services.AddScoped<IGenerativeIntegrationServices, ChatGptIntegrationService>();
services.AddScoped<ITransactionalIntegrationServices, TwilioIntegrationService>();
services.AddScoped<IReminderService, ReminderService>();
services.AddScoped<ISchedulerIntegrationRunner, SchedulerIntegrationRunner>();
services.AddScoped<IAgentService, SchedulerParserAgent>();
services.AddScoped<IAgentService, IntentClassifierAgent>();
services.AddScoped<IAgentService, ContactEnricherAgent>();

services.AddHttpContextAccessor();

var app = builder.Build();

app.UseRouting();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<UsageLimitMiddleware>();

app.UseAuthorization();

app.MapControllers();
app.MapOpenApi();
app.MapScalarApiReference();

app.Run();