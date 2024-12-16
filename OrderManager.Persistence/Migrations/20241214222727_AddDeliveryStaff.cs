using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryStaffs",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CurrentLocation = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CurrentOrderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStaffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryStaffs_AspNetUsers_Id",
                        column: x => x.Id,
                        principalSchema: "security",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryStaffs_Orders_CurrentOrderId",
                        column: x => x.CurrentOrderId,
                        principalSchema: "public",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStaffs_CurrentOrderId",
                schema: "public",
                table: "DeliveryStaffs",
                column: "CurrentOrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryStaffs",
                schema: "public");
        }
    }
}
