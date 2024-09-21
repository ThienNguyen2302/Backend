using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDTU.API.Migrations
{
    /// <inheritdoc />
    public partial class databasev3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_order_status",
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
                    table.PrimaryKey("PK_tb_order_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_internship_orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegistrationId = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusId = table.Column<string>(type: "text", nullable: true),
                    Company = table.Column<string>(type: "text", nullable: false),
                    TaxCode = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_tb_internship_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_internship_orders_tb_internship_registrations_Registrati~",
                        column: x => x.RegistrationId,
                        principalTable: "tb_internship_registrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_internship_orders_tb_order_status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "tb_order_status",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_orders_RegistrationId",
                table: "tb_internship_orders",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_internship_orders_StatusId",
                table: "tb_internship_orders",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_internship_orders");

            migrationBuilder.DropTable(
                name: "tb_order_status");
        }
    }
}
