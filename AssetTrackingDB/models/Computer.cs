
namespace AssetTracking;

public class Computer : Asset
{
  public Computer() { }

  public Computer(int id, string brand, string model, DateTime dateOfPurchase, float price, Office office)
      : base(id, brand, model, dateOfPurchase, price, office)
  {
  }
}