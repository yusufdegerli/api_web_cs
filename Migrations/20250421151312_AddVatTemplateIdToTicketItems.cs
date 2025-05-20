using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_for_sambapos.Migrations
{
    /// <inheritdoc />
    public partial class AddVatTemplateIdToTicketItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "TicketItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VatTemplateId",
                table: "TicketItems",
                type: "int",
                nullable: false,
                defaultValue: 0);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "TicketItems");

            migrationBuilder.DropColumn(
                name: "VatTemplateId",
                table: "TicketItems");*/
        }
    }
}
