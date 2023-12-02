using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "customer_id1",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "product_id1",
                table: "Purchases");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "customer_id1",
                table: "Purchases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "product_id1",
                table: "Purchases",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
