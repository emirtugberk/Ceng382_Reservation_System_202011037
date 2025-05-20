using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationFKToFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ReservationId",
                table: "Feedbacks",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Reservations_ReservationId",
                table: "Feedbacks",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Reservations_ReservationId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ReservationId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Feedbacks");
        }
    }
}
