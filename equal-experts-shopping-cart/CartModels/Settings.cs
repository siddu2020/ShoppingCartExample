namespace CartModels;

public class TaxSettings
{
    public decimal TaxOnProduct { get; set; }

    public TaxSettings(decimal taxOnProduct)
    {
        TaxOnProduct = taxOnProduct;
    }

    public TaxSettings()
    {
        // Adding Default Tax Settings
    }
}