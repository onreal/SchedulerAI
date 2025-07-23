namespace SchedulerApi.Application.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireApiKeyAttribute : Attribute
{
}