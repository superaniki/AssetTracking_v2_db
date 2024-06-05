// See https://aka.ms/new-console-template for more information
using AssetTracking;
using EntityDBTest;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
DotNetEnv.Env.Load();

//Office office = context.Offices.Include(x => x.Assets).SingleOrDefault(x => x.Country == "Sweden");
/*
foreach (var item in office!.Assets)
{
  Console.WriteLine(item.GetType().Name); // Phone
}
*/

AssetHandler handler = new();
handler.MainMenu();

// CREATE DATA - INSERT
/*Office newOffice = new Office();
newOffice.Country = "TJoland";
//newOffice.Id = 3;
context.Add(newOffice);
context.SaveChanges();
*/

//Office office 
/*
using EF1ToMRelationship;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("One To Many-Many To One Relationship!");

MyDbContext Context = new MyDbContext();

// GET DATA (READ) - SELECT
Bus MyBus = Context.Busses.Include(x => x.PassengersList).SingleOrDefault(x => x.Id == 1);

foreach (var item in MyBus.PassengersList)
{
    Console.WriteLine(item.Name);
}
*/