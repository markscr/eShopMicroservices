namespace Catalog.API.Features.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    List<string> Category,
    string ImageFile,
    decimal Price
) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool Success);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(p => p.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price is invalid");
    }
}

public class UpdateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(
        UpdateProductCommand command,
        CancellationToken cancellationToken
    )
    {
        Product newProduct = command.Adapt<Product>();
        Product? dbProduct =
            await session.LoadAsync<Product>(newProduct.Id, cancellationToken)
            ?? throw new ProductNotFoundException(command.Id);

        session.Update(newProduct);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
