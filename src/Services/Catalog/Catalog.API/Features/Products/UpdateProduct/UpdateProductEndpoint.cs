using Catalog.API.Features.Products.CreateProduct;

namespace Catalog.API.Features.Products.UpdateProduct;

public record UpdateProductRequest(
    string Name,
    string Description,
    List<string> Category,
    string ImageFile,
    decimal Price
);

public record UpdateProductResponse(bool Success);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/products",
                async (UpdateProductCommand command, ISender sender) =>
                {
                    UpdateProductResult result = await sender.Send(command);

                    return Results.Ok(new UpdateProductResponse(result.Success));
                }
            )
            .WithName("DeleteProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete product")
            .WithDescription("Delete product");
    }
}
