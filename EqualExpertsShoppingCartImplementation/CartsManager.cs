using CartModels;
using EqualExpertsShoppingCartAbstract;

namespace EqualExpertsShoppingCartImplementation;

public class CartsManager: ICartsManager
{
    private HashSet<ShoppingCart> ShoppingCarts { get; init; }
    
    public CartsManager()
    {
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
}