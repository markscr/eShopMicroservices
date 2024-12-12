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

public class CreateBasketCommandHandler : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(
        CreateBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        return new CreateBasketResult(command.ShoppingCart.UserName);
    }
}
