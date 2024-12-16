using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTotalPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OrderTotalPrice",
                schema: "public",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotalPrice",
                schema: "public",
                table: "Orders");
        }
    }
}
