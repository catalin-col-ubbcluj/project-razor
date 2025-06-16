using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_razor.Migrations
{
    /// <inheritdoc />
    public partial class AddClientIdToToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Audience",
                table: "Tokens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audience",
                table: "Tokens");
        }
    }
}
