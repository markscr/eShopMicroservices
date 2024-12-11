namespace Catalog.API.Features.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    List<string> Category,
    string ImageFile,
    decimal Price
) : ICommand<CreateProductCommandResult>;

public record CreateProductCommandResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(p => p.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price is invalid");
    }
}

internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductCommandResult>
{
    public async Task<CreateProductCommandResult> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken
    )
    {
        Product product = new()
        {
            Name = command.Name,
            Description = command.Description,
            Category = command.Category,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductCommandResult(product.Id);
    }
}
