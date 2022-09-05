namespace Neo.Infrastructure.Framework.Swagger;

public class SwaggerOptions
{
    public string BasePath { get; set; }
    public bool Enabled { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string Version { get; set; }
    public string RoutePrefix { get; set; }
    public bool IncludeSecurity { get; set; }
}