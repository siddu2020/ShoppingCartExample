using System.Net;
using CartModels;
using CartModels.Exceptions;
using EqualExpertsShoppingCartImplementation;
using RichardSzalay.MockHttp;

namespace TestShoppingCart;

public class CartManagerTest
{
   
    
    [Fact]
    public void AddProductInfo_Should_AddProductToCart()
    {
        // Arrange
        var cart = new ShoppingCart();
        var settings = new TaxSettings(0.125m);
        var messageHandler = new MockHttpMessageHandler();
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-1.json")
            .Respond("application/json", "{\"price\": 10.99, \"title\": \"Product-1\"}");
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-2.json")
            .Respond("application/json", "{\"price\": 5.99, \"title\": \"Product-2\"}");

        var cartManager = new CartManager(settings, cart, new HttpClient(messageHandler));
        var productName = "Product-1";
        var price = 10.99m;
        var quantity = 2;

        // Act
        cartManager.AddProductInfo(productName, quantity);

        // Assert
        Assert.Single(cart.ShoppingCartItems);
        var product = cart.ShoppingCartItems.First();
        Assert.Equal(productName, product.ProductInfo.Title);
        Assert.Equal(price, product.ProductInfo.Price);
        Assert.Equal(quantity, product.Quantity);
    }
    
    [Fact]
    public void AddProductInfo_Should_AddProductToCart_ForAproductThatAlreadyExists()
    {
        // Arrange
        var cart = new ShoppingCart();
        var settings = new TaxSettings(0.125m);
        var messageHandler = new MockHttpMessageHandler();
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-1.json")
            .Respond("application/json", "{\"price\": 10.99, \"title\": \"Product-1\"}");
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-2.json")
            .Respond("application/json", "{\"price\": 5.99, \"title\": \"Product-2\"}");

        var cartManager = new CartManager(settings, cart, new HttpClient(messageHandler));
        var productName = "Product-1";
        var price = 10.99m;
        var quantity = 2;

        // Act
        cartManager.AddProductInfo(productName, quantity);
        var product = cart.ShoppingCartItems.First();
        Assert.Equal(productName, product.ProductInfo.Title);
        Assert.Equal(price, product.ProductInfo.Price);
        Assert.Equal(quantity, product.Quantity);
        cartManager.AddProductInfo(productName, quantity);
         product = cart.ShoppingCartItems.First();

        // Assert
       
        Assert.Equal(quantity*2, product.Quantity);
    }

    [Fact]
    public void CalculateTax_Should_ReturnCorrectTaxAmount()
    {
        // Arrange
        var cart = new ShoppingCart();
        var settings = new TaxSettings(0.125m);
        var messageHandler = new MockHttpMessageHandler();
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-1.json")
            .Respond("application/json", "{\"price\": 10.99, \"title\": \"Product-1\"}");
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-2.json")
            .Respond("application/json", "{\"price\": 5.99, \"title\": \"Product-2\"}");
        var cartManager = new CartManager(settings, cart, new HttpClient(messageHandler));
        cartManager.AddProductInfo("Product-1",  2);
        cartManager.AddProductInfo("Product-2",  3);

        // Act
        var taxAmount = cartManager.CalculateTax();

        // Assert
        Assert.Equal(4.99375m, taxAmount);
    }
    
    [Fact ]
    public void CalculateTax_Should_ThrowProductNotFoundException()
    {
        // Arrange
        var cart = new ShoppingCart();
        var settings = new TaxSettings(0.125m);
        var messageHandler = new MockHttpMessageHandler();
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-1.json")
            .Respond("application/json", "{\"price\": 10.99, \"title\": \"Product-1\"}");
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-2.json")
            .Respond("application/json", "{\"price\": 5.99, \"title\": \"Product-2\"}");
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-3.json")
            .Respond(HttpStatusCode.NotFound);
        var cartManager = new CartManager(settings, cart, new HttpClient(messageHandler));
        Assert.Throws<ProductNotFoundException>(()=>cartManager.AddProductInfo("Product-3",  2));

    }

    [Fact]
    public void GenerateDetailedInvoice_Should_ReturnInvoiceWithProductDetails()
    {
        // Arrange
        var cart = new ShoppingCart();
        var settings = new TaxSettings(0.125m);
        var messageHandler = new MockHttpMessageHandler();
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-1.json")
            .Respond("application/json", "{\"price\": 10.99, \"title\": \"Product-1\"}");
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-2.json")
            .Respond("application/json", "{\"price\": 5.99, \"title\": \"Product-2\"}");
        var cartManager = new CartManager(settings, cart, new HttpClient(messageHandler));
        cartManager.AddProductInfo("Product-1",  2);
        cartManager.AddProductInfo("Product-2",  3);

        // Act
        var invoice = cartManager.GenerateDetailedInvoice();

        // Assert
        Assert.Equal(5, invoice.Count());
        Assert.Equal("Cart Contains 2 * Product-1", invoice.ElementAt(0));
        Assert.Equal("Cart Contains 3 * Product-2", invoice.ElementAt(1));
        Assert.Equal($"Subtotal: {39.95m:c}", invoice.ElementAt(2));
        Assert.Equal($"Tax: {4.99m:c}", invoice.ElementAt(3));
        Assert.Equal($"Total: {44.94m:c}", invoice.ElementAt(4));
    }
    [Fact]
    public void RemoveProductInfo_Should_UpdateProductQuantityInCart()
    {
        // Arrange
        var cart = new ShoppingCart();
        var settings = new TaxSettings(0.125m);
        var messageHandler = new MockHttpMessageHandler();
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-1.json")
            .Respond("application/json", "{\"price\": 10.99, \"title\": \"Product-1\"}");
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-2.json")
            .Respond("application/json", "{\"price\": 5.99, \"title\": \"Product-2\"}");

        var cartManager = new CartManager(settings, cart, new HttpClient(messageHandler));
        var productName = "Product-1";
        var price = 10.99m;
        var quantity = 2;

        // Act
        cartManager.AddProductInfo(productName, quantity);
        cartManager.RemoveProductInfo(productName, 1);

        // Assert
        Assert.Single(cart.ShoppingCartItems);
        var product = cart.ShoppingCartItems.First();
        Assert.Equal(productName, product.ProductInfo.Title);
        Assert.Equal(price, product.ProductInfo.Price);
        Assert.Equal(1, product.Quantity);
    }

    [Fact]
    public void RemoveProductInfo_Should_Not_FindTheProductInCart()
    {
        // Arrange
        var cart = new ShoppingCart();
        var settings = new TaxSettings(0.125m);
        var messageHandler = new MockHttpMessageHandler();
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-1.json")
            .Respond("application/json", "{\"price\": 10.99, \"title\": \"Product-1\"}");
        messageHandler.When("https://equalexperts.github.io/backend-take-home-test-data/Product-2.json")
            .Respond("application/json", "{\"price\": 5.99, \"title\": \"Product-2\"}");

        var cartManager = new CartManager(settings, cart, new HttpClient(messageHandler));
        var productName = "Product-1";
        var quantity = 2;

        // Act
        cartManager.AddProductInfo(productName, quantity);
        cartManager.RemoveProductInfo(productName, 2);

        // Assert
        Assert.Empty(cart.ShoppingCartItems);
    }
}