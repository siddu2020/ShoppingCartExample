using CartModels;
using EqualExpertsShoppingCartAbstract;
using EqualExpertsShoppingCartImplementation;
using Microsoft.AspNetCore.Mvc;

namespace EqualExpertsShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private TaxSettings TaxSettings { get; init; }
        private ICartsManager CartsManager { get; init; }
        private HttpClient Client { get; init; }
        public CartController(ICartsManager cartsManager, TaxSettings taxSettings, IHttpClientFactory clientFactory)
        {
            CartsManager = cartsManager;
            TaxSettings = taxSettings;
            Client = clientFactory.CreateClient("products");
        }
        
        [HttpPut("/create")]
        public ActionResult<ShoppingCart> CreateCart()
        {
            var cart = CartsManager.CreateCart();
            return Ok(cart);
        }

        [HttpGet("{id}")]
        public ActionResult<ShoppingCart> GetCartById(string id)
        {
            var cart = CartsManager.GetCartById(id);
            if(cart == null)
                return NotFound();
            return Ok(cart);
        }

        [HttpPut("addProduct/{id}/{productName}/{quantity}")]
        public ActionResult AddProductToCart(string id,string productName, int quantity)
        {
            try
            {
                var cart = CartsManager.GetCartById(id);
                var cartManager = new CartManager(TaxSettings, cart, Client);
                cartManager.AddProductInfo(productName, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        
        [HttpPatch("removeProduct/{id}/{productName}/{quantity}")]
        public ActionResult RemoveProductFromCart(string id, string productName, int quantity)
        {
            try
            {
                var cart = CartsManager.GetCartById(id);
                var cartManager = new CartManager(TaxSettings, cart, Client);
                cartManager.RemoveProductInfo(productName, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetInvoice/{id}")]
        public ActionResult<IEnumerable<string>> GetInvoice(string id)
        {
            try
            {
                var cart = CartsManager.GetCartById(id);
                var cartManager = new CartManager(TaxSettings, cart, Client);
                var invoice = cartManager.GenerateDetailedInvoice();
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
