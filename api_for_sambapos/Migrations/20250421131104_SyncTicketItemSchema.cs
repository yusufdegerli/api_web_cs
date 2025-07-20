using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_for_sambapos.Migrations
{
    /// <inheritdoc />
    public partial class SyncTicketItemSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "TicketItems",
                type: "int",
                nullable: false,
                defaultValue: 0);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "TicketItems");*/
        }
    }
}
