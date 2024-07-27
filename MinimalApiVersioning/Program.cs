using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using MinimalApiVersioning.Endpoints;
using MinimalApiVersioning.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1); // Default to version 1
    options.ReportApiVersions = true; // Include version info in response headers
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Read version from URL segment
})
.AddApiExplorer(options => 
{
    options.GroupNameFormat = "'v'V"; // Format for version grouping (e.g., v1, v2)
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

var app = builder.Build();

app.MapProductsEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Configure Swagger with API version descriptions
        IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

        foreach (ApiVersionDescription description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHttpsRedirection();

app.Run();