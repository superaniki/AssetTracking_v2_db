using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetTrackingDB.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExchangeRateFromDollar = table.Column<float>(type: "real", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfPurchase = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    AssetType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Country", "Currency", "ExchangeRateFromDollar" },
                values: new object[,]
                {
                    { 1, "Sweden", "SEK", 0.02f },
                    { 2, "USA", "USD", 1f },
                    { 3, "Greece", "EUR", 0.92f }
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetType", "Brand", "DateOfPurchase", "Model", "OfficeId", "Price" },
                values: new object[,]
                {
                    { 1, "Computer", "ASUS ROG", new DateTime(2021, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "B550-F", 1, 243f },
                    { 2, "Computer", "HP", new DateTime(2022, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "14S-FQ1010NO", 2, 679f },
                    { 3, "Phone", "Samsung", new DateTime(2023, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "S20 Plus", 3, 1500f },
                    { 4, "Phone", "Sony Xperia", new DateTime(2020, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "10 III", 2, 800f },
                    { 5, "Phone", "IPhone", new DateTime(2021, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10", 3, 951f },
                    { 6, "Computer", "HP", new DateTime(2021, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elitebook", 3, 2234f },
                    { 7, "Computer", "HP", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elitebook", 1, 3234f }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_OfficeId",
                table: "Assets",
                column: "OfficeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
