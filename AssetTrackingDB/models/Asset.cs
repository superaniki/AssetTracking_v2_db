
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracking;



public class Asset
{
  public enum ExpirationStatus
  {
    Overdue, //grey 
    Warning_Level2, //red
    Warning_Level1, //yellow
    Status_Default //white
  }

  public int Id { get; set; }  // Primary Key
  public string Brand { get; set; } = "";
  public string Model { get; set; } = "";
  public DateTime DateOfPurchase { get; set; }
  public float Price { get; set; }
  public Office Office { get; set; }  // Navigation Property : blir OfficeId
  public int OfficeId { get; set; }  // Primary Key

  [NotMapped]
  public ExpirationStatus Status { get; set; } = ExpirationStatus.Status_Default;

  public Asset() { }

  public Asset(int id, string brand, string model, DateTime dateOfPurchase, float price, Office office)
  {
    Id = id;
    Brand = brand;
    Model = model;
    DateOfPurchase = dateOfPurchase;
    Price = price;
    Office = office;
  }

  public void Deconstruct(out string brand, out string model, out DateTime date, out float price, out string office, out string currency)
  {
    brand = Brand;
    model = Model;
    date = DateOfPurchase;
    price = Price * Office.ExchangeRateFromDollar;
    office = Office.Country;
    currency = Office.Currency;
  }


  public void UpdateWarningLevel(DateTime today)
  {
    DateTime threeYearsAgo = today.AddYears(-3);
    DateTime threeMonthsUntilThreeYears = threeYearsAgo.AddMonths(+3);
    DateTime sixMonthsUntilThreeYears = threeYearsAgo.AddMonths(+6);

    // mindre än 3 månader kvar tills 3 år lammal
    if (DateOfPurchase <= threeMonthsUntilThreeYears && DateOfPurchase >= threeYearsAgo)
      Status = Asset.ExpirationStatus.Warning_Level2; // red
    else if (DateOfPurchase <= sixMonthsUntilThreeYears && DateOfPurchase >= threeYearsAgo)
      Status = Asset.ExpirationStatus.Warning_Level1; // yellow
    else if (DateOfPurchase >= threeYearsAgo)
      Status = Asset.ExpirationStatus.Status_Default; // grey
    else
      Status = Asset.ExpirationStatus.Overdue; // white

  }
}




