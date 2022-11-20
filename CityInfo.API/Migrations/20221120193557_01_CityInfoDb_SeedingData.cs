using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityInfo.API.Migrations
{
    public partial class _01_CityInfoDb_SeedingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "Description", "Name" },
                values: new object[] { 1, "The best city in usa", "New York City" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "Description", "Name" },
                values: new object[] { 2, "Capital of Indunisia", "Jakartha" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "Description", "Name" },
                values: new object[] { 3, "A Nice city of Japan", "Tokyo" });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "PointOfInterestId", "CityId", "Description", "Name" },
                values: new object[] { 1, 1, "There are so many trees", "Central park" });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "PointOfInterestId", "CityId", "Description", "Name" },
                values: new object[] { 2, 1, "There are so many people", "Central Market" });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "PointOfInterestId", "CityId", "Description", "Name" },
                values: new object[] { 3, 2, "There are so many buses", "Transport Center" });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "PointOfInterestId", "CityId", "Description", "Name" },
                values: new object[] { 4, 3, "There are so many sea food", "Fish Market" });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "PointOfInterestId", "CityId", "Description", "Name" },
                values: new object[] { 5, 2, "There are so many boats", "Harbor" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "PointOfInterestId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "PointOfInterestId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "PointOfInterestId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "PointOfInterestId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "PointOfInterestId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: 3);
        }
    }
}
