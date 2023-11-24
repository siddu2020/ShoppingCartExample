using Microsoft.AspNetCore.Mvc;

namespace EqualExpertsShoppingCartAPITests;

public class CartControllerTest
{
        [Fact]
        public void GetCart_ReturnsOkResult()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var mockClient = new Mock<IHttpClientFactory>();
            var controller = new CartController(cartsManagerMock.Object, taxSettings,mockClient.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns(cart);
            // Act
            var result = controller.GetCartById(cart.Id.ToString());
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCart_ReturnsCorrectCart()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var mockClient = new Mock<IHttpClientFactory>();
            var expectedCart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };

            cartsManagerMock
                .Setup(c => c.GetCartById(expectedCart.Id.ToString()))
                .Returns(expectedCart);
            var controller = new CartController(cartsManagerMock.Object, taxSettings, mockClient.Object);

            // Act
            var result = controller
                .GetCartById(expectedCart.Id.ToString())
                .Result as OkObjectResult;

            // Assert
            Assert.Equal(expectedCart, result.Value);
        }

        [Fact]
        public void GetCart_ReturnsNotFoundResult()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var mockClient = new Mock<IHttpClientFactory>();
            var controller = new CartController(cartsManagerMock.Object, taxSettings, mockClient.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns((ShoppingCart)null);
            // Act
            var result = controller.GetCartById(Guid.NewGuid().ToString());
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void CreateCart_ReturnsOkResult()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var mockClient = new Mock<IHttpClientFactory>();
            var controller = new CartController(cartsManagerMock.Object, taxSettings, mockClient.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            cartsManagerMock.Setup(c => c.CreateCart()).Returns(cart);
            // Act
            var result = controller.CreateCart();
            // Assert
            cartsManagerMock.Verify(c => c.CreateCart(), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cart, ((OkObjectResult)result.Result).Value);
        }
        
        [Fact]
        public void AddProductToCart_ReturnsOkResult()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var mockClient = new Mock<IHttpClientFactory>();
            var controller = new CartController(cartsManagerMock.Object, taxSettings, mockClient.Object);
            mockClient.Setup(c => c.CreateClient("products")).Returns(new HttpClient());
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns(cart);
            // Act
            var result = controller.AddProductToCart(cart.Id.ToString(), "cheerios", 2);
            // Assert
            cartsManagerMock.Verify(c => c.GetCartById(cart.Id.ToString()), Times.Once);
            Assert.IsType<OkResult>(result);
        }

}
