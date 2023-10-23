using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGCardsAPI.Migrations
{
    /// <inheritdoc />
    public partial class CardTypeNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardCardType_Types_TypeId",
                table: "CardCardType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Types",
                table: "Types");

            migrationBuilder.RenameTable(
                name: "Types",
                newName: "CardTypes");

            migrationBuilder.AlterColumn<string>(
                name: "FlavourText",
                table: "Cards",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 50000,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardTypes",
                table: "CardTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardCardType_CardTypes_TypeId",
                table: "CardCardType",
                column: "TypeId",
                principalTable: "CardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardCardType_CardTypes_TypeId",
                table: "CardCardType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardTypes",
                table: "CardTypes");

            migrationBuilder.RenameTable(
                name: "CardTypes",
                newName: "Types");

            migrationBuilder.AlterColumn<string>(
                name: "FlavourText",
                table: "Cards",
                type: "nvarchar(max)",
                maxLength: 50000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Types",
                table: "Types",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardCardType_Types_TypeId",
                table: "CardCardType",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
