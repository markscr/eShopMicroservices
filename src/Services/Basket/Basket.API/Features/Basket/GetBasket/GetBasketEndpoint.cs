namespace Basket.API.Features.Basket.GetBasket;

public record GetBasketRequest(string UserName);

public record GetBasketResponse(ShoppingCart ShoppingCart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/basket/{userName}",
                async (string userName, ISender sender) =>
                {
                    //GetBasketQuery query = request.Adapt<GetBasketQuery>();
                    GetBasketResult result = await sender.Send(new GetBasketQuery(userName));
                    GetBasketResponse response = result.Adapt<GetBasketResponse>();

                    return Results.Ok(response);
                }
            )
            .WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Basket")
            .WithDescription("Get Basket");
        ;
    }
}
