using BlazorSpinnerHosted.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorSpinnerHosted.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<SpinnerService>();
            builder.Services.AddScoped<BlazorDisplaySpinnerAutomaticallyHttpMessageHandler>();
            builder.Services.AddScoped(s =>
            {
                var accessTokenHandler = s.GetRequiredService<BlazorDisplaySpinnerAutomaticallyHttpMessageHandler>();
                accessTokenHandler.InnerHandler = new HttpClientHandler();
                var uriHelper = s.GetRequiredService<NavigationManager>();
                return new HttpClient(accessTokenHandler)
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });

            await builder.Build().RunAsync();
        }
    }
}
