using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class CreateOrderTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    User = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderFlightRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FlightRateId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderFlightRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderFlightRates_FlightRates_FlightRateId",
                        column: x => x.FlightRateId,
                        principalTable: "FlightRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderFlightRates_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderFlightRates_FlightRateId",
                table: "OrderFlightRates",
                column: "FlightRateId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFlightRates_OrderId",
                table: "OrderFlightRates",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderFlightRates");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
