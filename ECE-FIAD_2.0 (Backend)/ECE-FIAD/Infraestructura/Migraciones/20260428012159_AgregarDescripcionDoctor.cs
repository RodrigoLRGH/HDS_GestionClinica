using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarDescripcionDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Doctores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // ← borra todo el bloque de FechaContratacion de aquí
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Doctores");

            // ← borra todo el bloque de FechaContratacion de aquí también
        }
    }
}
