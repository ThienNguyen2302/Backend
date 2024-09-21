using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDTU.API.Migrations
{
    /// <inheritdoc />
    public partial class databasev7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "tb_regular_job_applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "tb_regular_job_applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryMax",
                table: "tb_regular_job_applications",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryMin",
                table: "tb_regular_job_applications",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "tb_internship_job_applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "tb_internship_job_applications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "tb_regular_job_applications");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "tb_regular_job_applications");

            migrationBuilder.DropColumn(
                name: "SalaryMax",
                table: "tb_regular_job_applications");

            migrationBuilder.DropColumn(
                name: "SalaryMin",
                table: "tb_regular_job_applications");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "tb_internship_job_applications");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "tb_internship_job_applications");
        }
    }
}
