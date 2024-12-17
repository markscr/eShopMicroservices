namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<ShoppingCart> CreateBasket(
        ShoppingCart shoppingCart,
        CancellationToken cancellationToken = default
    )
    {
        session.Store(shoppingCart);
        await session.SaveChangesAsync(cancellationToken);
        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(
        string UserName,
        CancellationToken cancellationToken = default
    )
    {
        session.Delete<ShoppingCart>(UserName);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ShoppingCart> GetBasket(
        string UserName,
        CancellationToken cancellationToken = default
    )
    {
        ShoppingCart? shoppingCart = await session.LoadAsync<ShoppingCart>(
            UserName,
            cancellationToken
        );

        return shoppingCart is null ? throw new BasketNotFoundException(UserName) : shoppingCart;
    }
}
