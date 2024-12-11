namespace Catalog.API.Features.Products.GetProductByCategory;

public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products/category/{category}",
                async (string category, ISender sender) =>
                {
                    GetProductByCategoryResult result = await sender.Send(
                        new GetProductByCategoryQuery(category)
                    );

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
