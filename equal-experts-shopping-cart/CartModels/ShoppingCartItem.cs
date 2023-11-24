namespace CartModels;

public record ShoppingCartItem(ProductInfo ProductInfo, int Quantity);

public class ShoppingCart 
{
   public HashSet<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();

   public Guid Id { get; set; } = Guid.NewGuid();
}

