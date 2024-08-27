using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Backend.Migrations
{
    /// <inheritdoc />
    public partial class CantidadPrestamo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Prestamos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Prestamos");
        }
    }
}
