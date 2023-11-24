using System.Text.Json;
using CartModels;
using CartModels.Exceptions;
using EqualExpertsShoppingCartAbstract;

namespace EqualExpertsShoppingCartImplementation;

public class CartManager:ICartManager
{
    
    private ShoppingCart Cart { get; init; }
    private TaxSettings Settings { get; init; }
    private HttpClient Client { get; init; }

    public CartManager(TaxSettings settings, ShoppingCart cart, HttpClient httpClient)
    {
        Cart = cart;
        Settings = settings;
        Client = httpClient;
        if(string.IsNullOrEmpty(Client.BaseAddress?.ToString()))
            Client.BaseAddress = new Uri("https://equalexperts.github.io/backend-take-home-test-data/");
    }
    
    public bool AddProductInfo(string productName, int quantity)
    {
        // Check if the product already exists in the shopping cart
        var existingItem = Cart.ShoppingCartItems.FirstOrDefault(item => item.ProductInfo.Title == productName);
        var price = 0m;
        var taskGetData = Client.GetAsync($"{productName}.json");
        taskGetData.Wait();
        var response = taskGetData.Result;
        if (response.IsSuccessStatusCode)
        {
            var taskReadData = response.Content.ReadAsStringAsync();
            taskReadData.Wait();
            var data = taskReadData.Result;
            var productInfo = JsonSerializer.Deserialize<ProductInfo>(data);
            if (productInfo != null) price = productInfo.Price;
        }
        else 
        {
            throw new ProductNotFoundException($"Product {productName} not found");
        }

        if (existingItem == null)
            return Cart.ShoppingCartItems.Add(
                new ShoppingCartItem(new ProductInfo() { Price = price, Title = productName }, quantity));
        // If the product exists, increase the quantity
        Cart.ShoppingCartItems.Remove(existingItem);
        return Cart.ShoppingCartItems.Add(existingItem with { Quantity = existingItem.Quantity + quantity });

    }

    public bool RemoveProductInfo(string productName, int quantity)
    {
        var isUpdated = false;
        // Check if the product already exists in the shopping cart
        var existingItem = Cart.ShoppingCartItems.FirstOrDefault(item => item.ProductInfo.Title == productName);

        if (existingItem == null) return true;
        // If the product exists, decrease the quantity
        isUpdated = Cart.ShoppingCartItems.Remove(existingItem);
        if(existingItem.Quantity > quantity )
            isUpdated = Cart.ShoppingCartItems.Add(existingItem with { Quantity = existingItem.Quantity - quantity });

        return isUpdated;
    }

    public decimal CalculateTax()
    {
        return Cart.ShoppingCartItems
            .Select(cartShoppingCartItem => cartShoppingCartItem.ProductInfo.Price * cartShoppingCartItem.Quantity)
            .Select(total => total * Settings.TaxOnProduct).Sum();
    }

    public IEnumerable<string> GenerateDetailedInvoice()
    {
        var invoiceItems = new List<string>();
        var subtotal = 0m;
        foreach (var item in Cart.ShoppingCartItems)
        {
            subtotal += item.ProductInfo.Price * item.Quantity;
            var invoiceItem = $"Cart Contains {item.Quantity} * {item.ProductInfo.Title}";
            invoiceItems.Add(invoiceItem);
        }
        invoiceItems.Add($"Subtotal: {subtotal:C}");
        var tax = CalculateTax();
        var total = subtotal + tax;
        invoiceItems.Add($"Tax: {tax:C}");
        invoiceItems.Add($"Total: {total:C}");
        return invoiceItems;
    }
    
}
