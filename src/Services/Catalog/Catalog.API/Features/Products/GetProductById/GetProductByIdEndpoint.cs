namespace Catalog.API.Features.Products.GetProductById;

//public record GetProductByIdRequest(Guid Id);

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products/{id}",
                async (Guid id, ISender sender) =>
                {
                    GetProductByIdResult result = await sender.Send(new GetProductByIdQuery(id));

                    GetProductByIdResponse response = result.Adapt<GetProductByIdResponse>();

                    return Results.Ok(response);
                }
            )
            .WithName("GetProductById")
            .WithSummary("Get product by id")
            .WithDescription("Get product by id")
            .Produces<Product>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
