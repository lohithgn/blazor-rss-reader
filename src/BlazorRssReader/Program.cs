using Blazor.Extensions.Logging;
using Blazored.Storage;
using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorRssReader
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                services.AddLocalStorage();
                services.AddSingleton<Session>();
                services.AddSingleton<NotificationService>();
                services.AddSingleton<FeedService>();

                services.AddLogging(builder => builder
                    .AddBrowserConsole() 
                    .SetMinimumLevel(LogLevel.Information) 
                );


            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
