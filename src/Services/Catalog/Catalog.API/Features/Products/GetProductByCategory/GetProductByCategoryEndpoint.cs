namespace Catalog.API.Features.Products.GetProductByCategory;

public record GetProductByCategoryRequest(string Category, int? PageNumber = 1, int? PageSize = 10);

public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products/category",
                async ([AsParameters] GetProductByCategoryRequest request, ISender sender) =>
                {
                    GetProductByCategoryQuery query = request.Adapt<GetProductByCategoryQuery>();
                    GetProductByCategoryResult result = await sender.Send(query);

                    return Results.Ok(result.Adapt<GetProductByCategoryResponse>());
                }
            )
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Category")
            .WithDescription("Get Product By Category");
        ;
    }
}
