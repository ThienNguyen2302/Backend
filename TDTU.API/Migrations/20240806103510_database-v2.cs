using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDTU.API.Migrations
{
    /// <inheritdoc />
    public partial class databasev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_tb_users_Id",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_tb_users_Id",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "tb_students");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "tb_companies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "tb_students",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Major",
                table: "tb_students",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_students",
                table: "tb_students",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_companies",
                table: "tb_companies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "tb_application_status",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_application_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_internship_terms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_internship_terms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_registration_status",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_registration_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_regular_jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "numeric", nullable: false),
                    SalaryMax = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_regular_jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_regular_jobs_tb_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tb_companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tb_internship_jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    InternshipTermId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_internship_jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_internship_jobs_tb_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tb_companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_internship_jobs_tb_internship_terms_InternshipTermId",
                        column: x => x.InternshipTermId,
                        principalTable: "tb_internship_terms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tb_internship_registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InternshipTermId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusId = table.Column<string>(type: "text", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_internship_registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_internship_registrations_tb_internship_terms_InternshipT~",
                        column: x => x.InternshipTermId,
                        principalTable: "tb_internship_terms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_internship_registrations_tb_registration_status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "tb_registration_status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_internship_registrations_tb_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tb_regular_job_applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    CV = table.Column<string>(type: "text", nullable: false),
                    Introduce = table.Column<string>(type: "text", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_regular_job_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_regular_job_applications_tb_regular_jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "tb_regular_jobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_regular_job_applications_tb_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tb_internship_job_applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusId = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    CV = table.Column<string>(type: "text", nullable: false),
                    Introduce = table.Column<string>(type: "text", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_internship_job_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_internship_job_applications_tb_application_status_Status~",
                        column: x => x.StatusId,
                        principalTable: "tb_application_status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_internship_job_applications_tb_internship_jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "tb_internship_jobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_internship_job_applications_tb_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_job_applications_JobId",
                table: "tb_internship_job_applications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_job_applications_StatusId",
                table: "tb_internship_job_applications",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_job_applications_StudentId",
                table: "tb_internship_job_applications",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_jobs_CompanyId",
                table: "tb_internship_jobs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_jobs_InternshipTermId",
                table: "tb_internship_jobs",
                column: "InternshipTermId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_registrations_InternshipTermId",
                table: "tb_internship_registrations",
                column: "InternshipTermId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_registrations_StatusId",
                table: "tb_internship_registrations",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_registrations_StudentId",
                table: "tb_internship_registrations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_regular_job_applications_JobId",
                table: "tb_regular_job_applications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_regular_job_applications_StudentId",
                table: "tb_regular_job_applications",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_regular_jobs_CompanyId",
                table: "tb_regular_jobs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_companies_tb_users_Id",
                table: "tb_companies",
                column: "Id",
                principalTable: "tb_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_students_tb_users_Id",
                table: "tb_students",
                column: "Id",
                principalTable: "tb_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_companies_tb_users_Id",
                table: "tb_companies");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_students_tb_users_Id",
                table: "tb_students");

            migrationBuilder.DropTable(
                name: "tb_internship_job_applications");

            migrationBuilder.DropTable(
                name: "tb_internship_registrations");

            migrationBuilder.DropTable(
                name: "tb_regular_job_applications");

            migrationBuilder.DropTable(
                name: "tb_application_status");

            migrationBuilder.DropTable(
                name: "tb_internship_jobs");

            migrationBuilder.DropTable(
                name: "tb_registration_status");

            migrationBuilder.DropTable(
                name: "tb_regular_jobs");

            migrationBuilder.DropTable(
                name: "tb_internship_terms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_students",
                table: "tb_students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_companies",
                table: "tb_companies");

            migrationBuilder.RenameTable(
                name: "tb_students",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "tb_companies",
                newName: "Companies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Students",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Major",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_tb_users_Id",
                table: "Companies",
                column: "Id",
                principalTable: "tb_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_tb_users_Id",
                table: "Students",
                column: "Id",
                principalTable: "tb_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
