using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnalyticsService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalIncome = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalSavings = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BudgetUtilization = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CategoryAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analytics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analytics_UserId",
                table: "Analytics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Analytics_UserId_CategoryId",
                table: "Analytics",
                columns: new[] { "UserId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Analytics_UserId_Date",
                table: "Analytics",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Analytics_UserId_Date_Period",
                table: "Analytics",
                columns: new[] { "UserId", "Date", "Period" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Analytics_UserId_Period",
                table: "Analytics",
                columns: new[] { "UserId", "Period" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analytics");
        }
    }
}
