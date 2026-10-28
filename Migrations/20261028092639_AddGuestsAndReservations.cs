using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wtlab77postgresefcorepravotorova.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestsAndReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 10, 28, 9, 26, 38, 406, DateTimeKind.Utc).AddTicks(6026));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 10, 28, 9, 26, 38, 406, DateTimeKind.Utc).AddTicks(6038));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 10, 28, 9, 26, 38, 406, DateTimeKind.Utc).AddTicks(6041));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 10, 28, 9, 24, 48, 803, DateTimeKind.Utc).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 10, 28, 9, 24, 48, 803, DateTimeKind.Utc).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 10, 28, 9, 24, 48, 803, DateTimeKind.Utc).AddTicks(224));
        }
    }
}
