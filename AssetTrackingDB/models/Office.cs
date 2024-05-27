

using AssetTracking;

public class Office
{
    public int Id { get; set; }  // Primary Key
    public string Country { get; set; } = "";
    public float ExchangeRateFromDollar { get; set; }
    public string Currency { get; set; } = "";
    public List<Asset> Assets { get; set; }

    public void Deconstruct(out string country, out string currency, out float exchangeRate)
    {
        country = Country;
        currency = Currency;
        exchangeRate = ExchangeRateFromDollar;
    }
}