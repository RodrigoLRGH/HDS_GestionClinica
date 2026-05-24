using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Migraciones
{
    /// <inheritdoc />
    public partial class EliminarIndiceUnicoHistoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
            name: "IX_HistoriasClinicas_IdPaciente",
            table: "HistoriasClinicas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
            name: "IX_HistoriasClinicas_IdPaciente",
            table: "HistoriasClinicas",
            column: "IdPaciente",
            unique: true);
        }
    }
}
