using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfLib.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "spot",
                columns: table => new
                {
                    spot_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    spot_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    spot_lat = table.Column<decimal>(type: "decimal(15,7)", nullable: false),
                    spot_lon = table.Column<decimal>(type: "decimal(15,7)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__spot__330AF0F66245A61D", x => x.spot_id);
                });

            migrationBuilder.CreateTable(
                name: "maree",
                columns: table => new
                {
                    maree_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maree_coefficient = table.Column<int>(type: "int", nullable: false),
                    maree_heure = table.Column<TimeOnly>(type: "time", nullable: false),
                    maree_date = table.Column<DateOnly>(type: "date", nullable: false),
                    maree_moment = table.Column<bool>(type: "bit", nullable: false),
                    maree_hauteur = table.Column<double>(type: "float", nullable: false),
                    SpotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__maree__78B12A65FE2A31BD", x => x.maree_id);
                    table.ForeignKey(
                        name: "FK_maree_spot_SpotId",
                        column: x => x.SpotId,
                        principalTable: "spot",
                        principalColumn: "spot_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_maree_SpotId",
                table: "maree",
                column: "SpotId");

            migrationBuilder.CreateIndex(
                name: "UQ_spot_name_lat_lon",
                table: "spot",
                columns: new[] { "spot_name", "spot_lat", "spot_lon" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maree");

            migrationBuilder.DropTable(
                name: "spot");
        }
    }
}
