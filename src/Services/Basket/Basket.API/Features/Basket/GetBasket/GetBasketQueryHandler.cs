namespace Basket.API.Features.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart ShoppingCart);

public class GetBasketQueryValidator : AbstractValidator<GetBasketQuery>
{
    public GetBasketQueryValidator()
    {
        RuleFor(q => q.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class GetBasketQueryHandler(IBasketRepository basketRepository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<GetBasketResult> Handle(
        GetBasketQuery request,
        CancellationToken cancellationToken
    )
    {
        ShoppingCart shoppingCart = await _basketRepository.GetBasket(
            request.UserName,
            cancellationToken
        );
        return new GetBasketResult(shoppingCart);
    }
}
