using CartModels;
using EqualExpertsShoppingCartImplementation;
using Microsoft.Extensions.Options;
using Moq;

namespace TestShoppingCart;


public class CartsManagerTest
{
    [Fact]
    public void CreateCart_ShouldReturnNewCart()
    {
        // Arrange
        var taxSettings = new TaxSettings(0.2m);
        var optionsMock = new Mock<IOptions<TaxSettings>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        optionsMock.Setup(op => op.Value).Returns(taxSettings);
        var cartsManager = new CartsManager(optionsMock.Object, httpClientFactoryMock.Object);

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
        var taxSettings = new TaxSettings(0.2m);
        var optionsMock = new Mock<IOptions<TaxSettings>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        optionsMock.Setup(op => op.Value).Returns(taxSettings);
        var cartsManager = new CartsManager(optionsMock.Object, httpClientFactoryMock.Object);
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
        var taxSettings = new TaxSettings(0.2m);
        var optionsMock = new Mock<IOptions<TaxSettings>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        optionsMock.Setup(op => op.Value).Returns(taxSettings);
        var cartsManager = new CartsManager(optionsMock.Object, httpClientFactoryMock.Object);
        var cart = cartsManager.CreateCart();

        // Act
        var retrievedCart = cartsManager.GetCartById(cart.Id.ToString());

        // Assert
        Assert.NotNull(retrievedCart);
        Assert.Equal(cart.Id.ToString(), retrievedCart.Id.ToString());
    }
}