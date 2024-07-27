using Asp.Versioning;
using Asp.Versioning.Builder;

namespace MinimalApiVersioning.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoint(this IEndpointRouteBuilder app)
    {
        // Define the API versions supported by this endpoint set
        ApiVersionSet versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1)) // Version 1.0
            .HasApiVersion(new ApiVersion(2)) // Version 2.0
            .HasDeprecatedApiVersion(new ApiVersion(1)) // Mark version 1.0 as deprecated
            .Build();

        // Create a route group with the defined version set
        RouteGroupBuilder groupBuilder = app.MapGroup("api/v{apiVersion:apiVersion}")
            .WithApiVersionSet(versionSet);

        // GET /api/v1/products
        // For demo purposes, returns static strings. 
        groupBuilder.MapGet("products", () => "List of Products v1")
            .MapToApiVersion(1)
            .Produces<string>(StatusCodes.Status200OK);

        // GET /api/v2/products
        // For demo purposes, returns static strings. 
        groupBuilder.MapGet("products", () => "List of Products v2")
            .MapToApiVersion(2)
            .Produces<string>(StatusCodes.Status200OK);
    }
}