using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d952142-74d2-40f1-931c-58ef1063c8af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3103c37-668e-405b-8592-a1e8c0a7921b");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7169225d-5cba-4671-b0e4-a6159661b730", null, "Customer", "CUSTOMER" },
                    { "871c1ad4-254d-4234-a930-7d6162a7ff14", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7169225d-5cba-4671-b0e4-a6159661b730");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "871c1ad4-254d-4234-a930-7d6162a7ff14");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d952142-74d2-40f1-931c-58ef1063c8af", null, "Customer", "CUSTOMER" },
                    { "b3103c37-668e-405b-8592-a1e8c0a7921b", null, "Admin", "ADMIN" }
                });
        }
    }
}
