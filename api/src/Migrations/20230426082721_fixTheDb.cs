using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace src.Migrations
{
    /// <inheritdoc />
    public partial class fixTheDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedUser");

            migrationBuilder.AddColumn<List<int>>(
                name: "ShareUsersId",
                table: "Projects",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "ShareUsersUsername",
                table: "Projects",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareUsersId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ShareUsersUsername",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "SharedUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedUser_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedUser_ProjectId",
                table: "SharedUser",
                column: "ProjectId");
        }
    }
}
