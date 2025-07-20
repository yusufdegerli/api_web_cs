using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_for_sambapos.Migrations
{
    public partial class AddGiftedAndLockedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AddColumn<bool>(
                name: "Gifted",
                table: "TicketItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "TicketItems",
                type: "bit",
                nullable: false,
                defaultValue: false);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropColumn(
                name: "Gifted",
                table: "TicketItems");

            migrationBuilder.DropColumn(
                name: "Locked",
                table: "TicketItems");*/
        }
    }
}