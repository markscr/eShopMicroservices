namespace Catalog.API.Features.Products.DeleteProduct;

public record DeleteProductResponse(bool Success);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/products/{id}",
            async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));

                return Results.NoContent();
            }
        );
    }
}
