using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Backend.Migrations
{
    /// <inheritdoc />
    public partial class DisponibilidadLibro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Libros",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Libros");
        }
    }
}
