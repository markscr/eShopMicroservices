using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    List<string> Category,
    string ImageFile,
    decimal Price
) : ICommand<CreateProductCommandResult>;

public record CreateProductCommandResult(Guid Id);

internal class CreateProductCommandHandler
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

        return new CreateProductCommandResult(Guid.NewGuid());
    }
}
