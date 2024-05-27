

namespace AssetTracking;


public class Phone : Asset
{
  public Phone() { }

  public Phone(int id, string brand, string model, DateTime dateOfPurchase, float price, Office office)
      : base(id, brand, model, dateOfPurchase, price, office)
  {
  }
}