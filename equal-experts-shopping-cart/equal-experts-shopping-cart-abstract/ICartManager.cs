namespace EqualExpertsShoppingCartAbstract;

public interface ICartManager
{
        bool AddProductInfo(string productName, int quantity);
        decimal CalculateTax();
        IEnumerable<string> GenerateDetailedInvoice();
        bool RemoveProductInfo(string productName, int quantity);
    
}


