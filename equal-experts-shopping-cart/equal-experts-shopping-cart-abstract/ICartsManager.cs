using CartModels;

namespace EqualExpertsShoppingCartAbstract;

public interface ICartsManager
{
    ShoppingCart CreateCart(); // Method to create a new cart

    void RemoveCart(string cartId); // Method to remove a cart by ID

    ShoppingCart GetCartById(string cartId); 
    
    ICartManager GetCartManagerById(string cartId);
}
