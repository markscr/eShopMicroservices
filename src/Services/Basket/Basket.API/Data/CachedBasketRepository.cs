using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        string? cachedBasket = await cache.GetStringAsync(userName, token: cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        ShoppingCart? shoppingCart = await basketRepository.GetBasket(userName, cancellationToken);
        await cache.SetStringAsync(
            userName,
            JsonSerializer.Serialize(shoppingCart),
            cancellationToken
        );

        return shoppingCart;
    }

    public async Task<ShoppingCart> CreateBasket(
        ShoppingCart shoppingCart,
        CancellationToken cancellationToken = default
    )
    {
        await basketRepository.CreateBasket(shoppingCart, cancellationToken);

        await cache.SetStringAsync(
            shoppingCart.UserName,
            JsonSerializer.Serialize(shoppingCart),
            cancellationToken
        );

        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        await basketRepository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);
        return true;
    }
}
