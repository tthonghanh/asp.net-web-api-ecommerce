using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddDatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24fd5556-025c-46a5-9232-773a08fd9429");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "879c71f4-79e9-4d79-9612-1e0dba23c51a");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "68641506-5739-44ed-bf6c-14ba98b63d21", null, "Customer", "CUSTOMER" },
                    { "81b90313-9e84-4b6d-8104-7e30ed93a4d5", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68641506-5739-44ed-bf6c-14ba98b63d21");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81b90313-9e84-4b6d-8104-7e30ed93a4d5");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24fd5556-025c-46a5-9232-773a08fd9429", null, "Admin", "ADMIN" },
                    { "879c71f4-79e9-4d79-9612-1e0dba23c51a", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
