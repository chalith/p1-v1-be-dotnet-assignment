using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AlterOrderTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Price_Currency",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Price_FlightRateId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price_Value",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price_Currency",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price_FlightRateId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price_Value",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
