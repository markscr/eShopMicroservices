using Basket.API.Features.Basket.GetBasket;

namespace Basket.API.Features.Basket.DeleteBasket;

public record DeleteBasketRequest(string UserName) : IQuery<DeleteBasketResponse>;

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/basket/{userName}",
                async (string userName, ISender sender) =>
                {
                    //DeleteBasketCommand command = request.Adapt<DeleteBasketCommand>();
                    DeleteBasketResult result = await sender.Send(
                        new DeleteBasketCommand(userName)
                    );
                    DeleteBasketResponse response = result.Adapt<DeleteBasketResponse>();

                    return Results.NoContent();
                }
            )
            .WithName("DeleteBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket");
        ;
    }
}
