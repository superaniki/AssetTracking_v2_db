

// string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=User;Integrated Security=True";


using AssetTracking;
using Microsoft.EntityFrameworkCore;

namespace EntityDBTest
{
  internal class MyDbContext : DbContext
  {
    string connectionString = Environment.GetEnvironmentVariable("MyConnectionString")!;

    //public DbSet<Asset> Assetts { set; get; }
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<Computer> Computers { get; set; } = null!;
    public DbSet<Phone> Phones { get; set; } = null!;
    public DbSet<Office> Offices { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      //connectionString = connectionString;
      optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Asset>()
          .HasDiscriminator<string>("AssetType")
          .HasValue<Computer>("Computer")
          .HasValue<Phone>("Phone");

      modelBuilder.Entity<Asset>()
          .HasOne(a => a.Office)
          .WithMany(o => o.Assets)
          .HasForeignKey(a => a.OfficeId);

      Office sweOffice = new Office { Id = 1, Country = "Sweden", Currency = "SEK", ExchangeRateFromDollar = 0.02f };
      Office usaOffice = new Office { Id = 2, Country = "USA", Currency = "USD", ExchangeRateFromDollar = 1 };
      Office greeceOffice = new Office { Id = 3, Country = "Greece", Currency = "EUR", ExchangeRateFromDollar = 0.92f };

      modelBuilder.Entity<Office>().HasData(sweOffice, usaOffice, greeceOffice);

      modelBuilder.Entity<Computer>().HasData(
          new Computer
          {
            Id = 1,
            Brand = "ASUS ROG",
            Model = "B550-F",
            DateOfPurchase = new DateTime(2021, 04, 03),
            Price = 243,
            OfficeId = 1
          },
          new Computer
          {
            Id = 2,
            Brand = "HP",
            Model = "14S-FQ1010NO",
            DateOfPurchase = new DateTime(2022, 01, 30),
            Price = 679,
            OfficeId = 2
          },
          new Computer
          {
            Id = 6,
            Brand = "HP",
            Model = "Elitebook",
            DateOfPurchase = new DateTime(2021, 08, 30),
            Price = 2234,
            OfficeId = 3
          },
          new Computer
          {
            Id = 7,
            Brand = "HP",
            Model = "Elitebook",
            DateOfPurchase = new DateTime(2021, 07, 30),
            Price = 3234,
            OfficeId = 1
          });

      modelBuilder.Entity<Phone>().HasData(
          new Phone
          {
            Id = 3,
            Brand = "Samsung",
            Model = "S20 Plus",
            DateOfPurchase = new DateTime(2023, 09, 12),
            Price = 1500,
            OfficeId = 3
          },
          new Phone
          {
            Id = 4,
            Brand = "Sony Xperia",
            Model = "10 III",
            DateOfPurchase = new DateTime(2020, 03, 06),
            Price = 800,
            OfficeId = 2
          },
          new Phone
          {
            Id = 5,
            Brand = "IPhone",
            Model = "10",
            DateOfPurchase = new DateTime(2021, 05, 01),
            Price = 951,
            OfficeId = 3
          });

      base.OnModelCreating(modelBuilder);
    }
  }
}


/*
ModelBuilder.Entity<Book>()
    .HasMany(x => x.AuthorsList)
    .WithMany(x => x.BooksList)
    .UsingEntity(x => x.HasData(new { AuthorsListId = 1, BooksListId = 1 }));

ModelBuilder.Entity<Book>()
  .HasMany(x => x.AuthorsList)
  .WithMany(x => x.BooksList)
  .UsingEntity(x => x.HasData(new { AuthorsListId = 2, BooksListId = 1 }));

ModelBuilder.Entity<Book>()
  .HasMany(x => x.AuthorsList)
  .WithMany(x => x.BooksList)
  .UsingEntity(x => x.HasData(new { AuthorsListId = 3, BooksListId = 2 }));

*/