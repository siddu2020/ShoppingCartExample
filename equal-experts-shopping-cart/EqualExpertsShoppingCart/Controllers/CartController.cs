using CartModels;
using CartModels.Exceptions;
using EqualExpertsShoppingCartAbstract;
using EqualExpertsShoppingCartImplementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EqualExpertsShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private TaxSettings TaxSettings { get; init; }
        private ICartsManager CartsManager { get; init; }
        public CartController(ICartsManager cartsManager, IOptions<TaxSettings> taxSettings)
        {
            CartsManager = cartsManager;
            TaxSettings = taxSettings.Value;
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
                var cartManager = CartsManager.GetCartManagerById(id);
                var result = cartManager.AddProductInfo(productName, quantity);
                return Ok(result);
            }
            catch (ProductNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }
        
        [HttpPatch("removeProduct/{id}/{productName}/{quantity}")]
        public ActionResult RemoveProductFromCart(string id, string productName, int quantity)
        {
            try
            {
                var cartManager = CartsManager.GetCartManagerById(id);
                var result = cartManager.RemoveProductInfo(productName, quantity);
                return Ok(result);
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
                var cartManager = CartsManager.GetCartManagerById(id);
                var invoice = cartManager.GenerateDetailedInvoice();
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
