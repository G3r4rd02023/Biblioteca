using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Backend.Migrations
{
    /// <inheritdoc />
    public partial class CorreoElectronico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NombreUsuario",
                table: "Usuarios",
                newName: "Correo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Correo",
                table: "Usuarios",
                newName: "NombreUsuario");
        }
    }
}
