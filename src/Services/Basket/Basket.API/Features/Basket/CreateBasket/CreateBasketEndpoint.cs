using Basket.API.Features.Basket.GetBasket;

namespace Basket.API.Features.Basket.CreateBasket;

public record CreateBasketRequest(ShoppingCart ShoppingCart) : IQuery<CreateBasketResponse>;

public record CreateBasketResponse(string UserName);

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/basket",
                async (CreateBasketRequest request, ISender sender) =>
                {
                    CreateBasketCommand command = request.Adapt<CreateBasketCommand>();
                    CreateBasketResult result = await sender.Send(command);
                    CreateBasketResponse response = result.Adapt<CreateBasketResponse>();

                    return Results.Created($"/basket/{response.UserName}", response);
                }
            )
            .WithName("CreateBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Basket")
            .WithDescription("Create Basket");
    }
}
