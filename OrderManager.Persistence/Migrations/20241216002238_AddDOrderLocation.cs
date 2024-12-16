using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDOrderLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderLocation",
                schema: "public",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderLocation",
                schema: "public",
                table: "Orders");
        }
    }
}
