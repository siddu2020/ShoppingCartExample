using CartModels;
using EqualExpertsShoppingCartAbstract;
using Microsoft.Extensions.Options;

namespace EqualExpertsShoppingCartImplementation;

public class CartsManager: ICartsManager
{
    private HashSet<ShoppingCart> ShoppingCarts { get; init; }
    private TaxSettings TaxSettings { get; init; }
    private HttpClient Client { get; init; }
    
    public CartsManager(IOptions<TaxSettings> taxSettings, IHttpClientFactory clientFactory)
    {
        TaxSettings = taxSettings.Value;
        Client = clientFactory.CreateClient("products");
        ShoppingCarts = new HashSet<ShoppingCart>();
    }
    

    public ShoppingCart CreateCart()
    {
        var cart = new ShoppingCart() {Id = Guid.NewGuid()};
        ShoppingCarts.Add(cart);
        return cart;
    }

    public void RemoveCart(string cartId)
    {
        ShoppingCarts.RemoveWhere(cart => cart.Id.ToString() == cartId);
    }

    public ShoppingCart GetCartById(string cartId)
    {
        return ShoppingCarts.FirstOrDefault(cart => cart.Id.ToString() == cartId);
    }

    public ICartManager GetCartManagerById(string cartId)
    {
        var cart = GetCartById(cartId);
        return new CartManager(TaxSettings, cart, Client);
    }
}