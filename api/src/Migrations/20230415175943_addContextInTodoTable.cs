using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Migrations
{
    /// <inheritdoc />
    public partial class addContextInTodoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateddAt",
                table: "Todos",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateddAt",
                table: "Projects",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateddAt",
                table: "Comments",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<string>(
                name: "Context",
                table: "Todos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Context",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Todos",
                newName: "UpdateddAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Projects",
                newName: "UpdateddAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Comments",
                newName: "UpdateddAt");
        }
    }
}
