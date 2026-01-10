using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForgottenConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseDevice_Warehouses_WarehouseId",
                table: "WarehouseDevice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseDevice",
                table: "WarehouseDevice");

            migrationBuilder.RenameTable(
                name: "WarehouseDevice",
                newName: "WarehouseDevices");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseDevice_WarehouseId",
                table: "WarehouseDevices",
                newName: "IX_WarehouseDevices_WarehouseId");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "WarehouseDevices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseDevices",
                table: "WarehouseDevices",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DeviceTelemetries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDoorOpen = table.Column<bool>(type: "bit", nullable: false),
                    IsPowerOn = table.Column<bool>(type: "bit", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: true),
                    Humidity = table.Column<double>(type: "float", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTelemetries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceTelemetries_WarehouseDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "WarehouseDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseDevices_DeviceId",
                table: "WarehouseDevices",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTelemetries_DeviceId_Timestamp",
                table: "DeviceTelemetries",
                columns: new[] { "DeviceId", "Timestamp" });

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseDevices_Warehouses_WarehouseId",
                table: "WarehouseDevices",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseDevices_Warehouses_WarehouseId",
                table: "WarehouseDevices");

            migrationBuilder.DropTable(
                name: "DeviceTelemetries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseDevices",
                table: "WarehouseDevices");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseDevices_DeviceId",
                table: "WarehouseDevices");

            migrationBuilder.RenameTable(
                name: "WarehouseDevices",
                newName: "WarehouseDevice");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseDevices_WarehouseId",
                table: "WarehouseDevice",
                newName: "IX_WarehouseDevice_WarehouseId");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "WarehouseDevice",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseDevice",
                table: "WarehouseDevice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseDevice_Warehouses_WarehouseId",
                table: "WarehouseDevice",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
