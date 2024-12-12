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

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(
        GetBasketQuery request,
        CancellationToken cancellationToken
    )
    {
        return new GetBasketResult(new ShoppingCart());
    }
}
