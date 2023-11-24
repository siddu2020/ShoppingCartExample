using EqualExpertsShoppingCartImplementation;

namespace TestShoppingCart;


public class CartsManagerTest
{
    [Fact]
    public void CreateCart_ShouldReturnNewCart()
    {
        // Arrange
        var cartsManager = new CartsManager();

        // Act
        var cart = cartsManager.CreateCart();

        // Assert
        Assert.NotNull(cart);
        Assert.Equal(0, cart.ShoppingCartItems.Count);
    }

    [Fact]
    public void RemoveCart_ShouldRemoveCart()
    {
        // Arrange
        var cartsManager = new CartsManager();
        var cart = cartsManager.CreateCart();

        // Act
        cartsManager.RemoveCart(cart.Id.ToString());

        // Assert
        Assert.Null(cartsManager.GetCartById(cart.Id.ToString()));
    }

    [Fact]
    public void GetCartById_ShouldReturnCart()
    {
        // Arrange
        var cartsManager = new CartsManager();
        var cart = cartsManager.CreateCart();

        // Act
        var retrievedCart = cartsManager.GetCartById(cart.Id.ToString());

        // Assert
        Assert.NotNull(retrievedCart);
        Assert.Equal(cart.Id.ToString(), retrievedCart.Id.ToString());
    }
}