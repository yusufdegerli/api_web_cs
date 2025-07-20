using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_for_sambapos.Migrations
{
    /// <inheritdoc />
    public partial class AddTableIdTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tables_TicketId",
                table: "Tables",
                column: "TicketId");

            // Foreign key kısıtlamasını kaldırıyoruz
            // migrationBuilder.AddForeignKey(
            //     name: "FK_Tables_Tickets_TicketId",
            //     table: "Tables",
            //     column: "TicketId",
            //     principalTable: "Tickets",
            //     principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Tables_Tickets_TicketId",
            //     table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_TicketId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Tickets");
        }
    }
}
