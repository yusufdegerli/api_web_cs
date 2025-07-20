using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_for_sambapos.Migrations
{
    /// <inheritdoc />
    public partial class AddIsClosedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Tickets");
        }
    }
}
