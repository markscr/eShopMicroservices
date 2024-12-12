namespace Catalog.API.Features.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category, int PageNumber = 1, int PageSize = 10)
    : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryQueryValidator : AbstractValidator<GetProductByCategoryQuery>
{
    public GetProductByCategoryQueryValidator()
    {
        RuleFor(p => p.Category).NotEmpty().WithMessage("Category is required");
    }
}

internal class GetProductByCategoryQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(
        GetProductByCategoryQuery query,
        CancellationToken cancellationToken
    )
    {
        IPagedList<Product> products = await session
            .Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToPagedListAsync(query.PageNumber, query.PageSize, cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}
