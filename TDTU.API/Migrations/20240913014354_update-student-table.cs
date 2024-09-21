using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDTU.API.Migrations
{
    /// <inheritdoc />
    public partial class updatestudenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "tb_student_profiles");

            migrationBuilder.DropColumn(
                name: "Skill",
                table: "tb_student_profiles");

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "tb_students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Skill",
                table: "tb_students",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "tb_students");

            migrationBuilder.DropColumn(
                name: "Skill",
                table: "tb_students");

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "tb_student_profiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Skill",
                table: "tb_student_profiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
