using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valera.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddValeraName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Valeras",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Valeras");
        }
    }
}
