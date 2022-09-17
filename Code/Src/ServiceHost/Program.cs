using ServiceHost;
using Microsoft.AspNetCore;

await CreateWebHostBuilder(args).Build().RunAsync();

static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();