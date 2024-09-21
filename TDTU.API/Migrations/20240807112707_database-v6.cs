using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDTU.API.Migrations
{
    /// <inheritdoc />
    public partial class databasev6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Medias",
                table: "Medias");

            migrationBuilder.RenameTable(
                name: "Medias",
                newName: "tb_medias");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_medias",
                table: "tb_medias",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_medias",
                table: "tb_medias");

            migrationBuilder.RenameTable(
                name: "tb_medias",
                newName: "Medias");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medias",
                table: "Medias",
                column: "Id");
        }
    }
}
