namespace Basket.API.Features.Basket.CreateBasket;

public record CreateBasketCommand(ShoppingCart ShoppingCart) : ICommand<CreateBasketResult>;

public record CreateBasketResult(string UserName);

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(c => c.ShoppingCart).NotNull().WithMessage("Shopping cart can not be null");
        RuleFor(c => c.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CreateBasketCommandHandler(IBasketRepository basketRepository)
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<CreateBasketResult> Handle(
        CreateBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        ShoppingCart? shoppingCart = await _basketRepository.CreateBasket(
            command.ShoppingCart,
            cancellationToken
        );
        return new CreateBasketResult(shoppingCart.UserName);
    }
}
