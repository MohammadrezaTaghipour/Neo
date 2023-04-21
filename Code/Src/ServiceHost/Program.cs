using Microsoft.AspNetCore;
using ServiceHost;

try
{
    await CreateWebHostBuilder(args).Build().RunAsync();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();
