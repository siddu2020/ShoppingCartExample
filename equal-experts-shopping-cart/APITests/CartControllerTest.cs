using CartModels.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EqualExpertsShoppingCartAPITests;

public class CartControllerTest
{
        [Fact]
        public void GetCart_ReturnsOkResult()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
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
            var expectedCart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };

            cartsManagerMock
                .Setup(c => c.GetCartById(expectedCart.Id.ToString()))
                .Returns(expectedCart);
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);

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
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
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
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
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
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            var cartManagerMock = new Mock<ICartManager>(); 
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns(cart);
            cartsManagerMock.Setup(c => c.GetCartManagerById(cart.Id.ToString()))
                .Returns(cartManagerMock.Object);
            
            // Act
            var result = controller.AddProductToCart(cart.Id.ToString(), "cheerios", 2);
            // Assert
            cartManagerMock.Verify(c => c.AddProductInfo("cheerios", 2), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void AddProductToCart_InvalidProductTest()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            var cartManagerMock = new Mock<ICartManager>(); 
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns(cart);
            cartsManagerMock.Setup(c => c.GetCartManagerById(cart.Id.ToString()))
                .Returns(cartManagerMock.Object);
            cartManagerMock.Setup(c => c.AddProductInfo("cheerios", 2)).Throws(new ProductNotFoundException("Product not found"));
            
            // Act
            var result = controller.AddProductToCart(cart.Id.ToString(), "cheerios", 2);
            // Assert
            cartManagerMock.Verify(c => c.AddProductInfo("cheerios", 2), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public void RemoveProductFromCart_ReturnsOkResult()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            var cartManagerMock = new Mock<ICartManager>(); 
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns(cart);
            cartsManagerMock.Setup(c => c.GetCartManagerById(cart.Id.ToString()))
                .Returns(cartManagerMock.Object);
            
            // Act
            var result = controller.RemoveProductFromCart(cart.Id.ToString(), "cheerios", 2);
            // Assert
            cartManagerMock.Verify(c => c.RemoveProductInfo("cheerios", 2), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void RemoveProductFromCart_ReturnsBadResult()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            var cartManagerMock = new Mock<ICartManager>(); 
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns(cart);
            cartsManagerMock.Setup(c => c.GetCartManagerById(cart.Id.ToString()))
                .Returns(cartManagerMock.Object);
            cartManagerMock.Setup(c => c.RemoveProductInfo("cheerios", 2)).Throws(new Exception("Something bad happened"));
            
            // Act
            var result = controller.RemoveProductFromCart(cart.Id.ToString(), "cheerios", 2);
            // Assert
            cartManagerMock.Verify(c => c.RemoveProductInfo("cheerios", 2), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public void GetInvoice_ReturnsOkResult()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            var cartManagerMock = new Mock<ICartManager>(); 
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns(cart);
            cartsManagerMock.Setup(c => c.GetCartManagerById(cart.Id.ToString()))
                .Returns(cartManagerMock.Object);
            var stringList = new List<string>();
            stringList.Add("Cart Contains 2 * Product-1");
            stringList.Add("Cart Contains 3 * Product-2");
            stringList.Add($"Subtotal: {39.95m:c}");
            stringList.Add($"Tax: {4.99m:c}");
            stringList.Add($"Total: {44.94m:c}");
            cartManagerMock.Setup(c => c.GenerateDetailedInvoice()).Returns(stringList);
            
            // Act
            var result = controller.GetInvoice(cart.Id.ToString());
            // Assert
            cartManagerMock.Verify(c => c.GenerateDetailedInvoice(), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(stringList, ((OkObjectResult)result.Result).Value as List<string>);
        }

        [Fact]
        public void GenerateInvoice_FailureScenario()
        {
            // Arrange
            var cartsManagerMock = new Mock<ICartsManager>();
            var taxSettings = new TaxSettings(0.125m);
            var optionsMock = new Mock<IOptions<TaxSettings>>();
            optionsMock.Setup(op => op.Value).Returns(taxSettings);
            var controller = new CartController(cartsManagerMock.Object, optionsMock.Object);
            var cart = new ShoppingCart()
            {
                ShoppingCartItems = new HashSet<ShoppingCartItem>(),
                Id = Guid.NewGuid()
            };
            var cartManagerMock = new Mock<ICartManager>(); 
            cartsManagerMock.Setup(c => c.GetCartById(cart.Id.ToString())).Returns(cart);
            cartsManagerMock.Setup(c => c.GetCartManagerById(cart.Id.ToString()))
                .Returns(cartManagerMock.Object);
            cartManagerMock.Setup(c => c.GenerateDetailedInvoice())
                .Throws(new Exception("Something bad happened"));
            
            // Act
            var result = controller.GetInvoice(cart.Id.ToString());
            // Assert
            cartManagerMock.Verify(c => c.GenerateDetailedInvoice(), Times.Once);
            Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal("Something bad happened", ((ObjectResult)result.Result).Value);
            Assert.Equal(500, ((ObjectResult)result.Result).StatusCode);
        }

}
