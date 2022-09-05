using ServiceHost;
using Microsoft.AspNetCore;
using Neo.Infrastructure.Framework.Swagger;


await CreateWebHostBuilder(args).Build().RunAsync();

static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .Configure(a => a.UseSwaggerDocs())
        .UseStartup<Startup>();