using System.Reflection;

namespace SchedulerApi.Application.Prompts.Helpers;

public class PromptHelper
{
    public static string GetPromptFromResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // var resources = assembly.GetManifestResourceNames();
        // Console.WriteLine("Available resources:");
        // foreach (var res in resources)
        // {
        //     Console.WriteLine(res);
        // }
        
        using var stream = assembly.GetManifestResourceStream(resourceName)
                           ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}