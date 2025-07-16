using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsAndStatusToTodoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "TodoItems",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "TodoItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TodoItems",
                newName: "IsCompleted");
        }
    }
}
