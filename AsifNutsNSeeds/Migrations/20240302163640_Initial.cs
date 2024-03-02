using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsifNutsNSeeds.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Branches_Branches_BranchID",
                table: "Product_Branches");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BranchID",
                table: "Product_Branches",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProducerID",
                table: "Producers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CountryID",
                table: "Countries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BranchID",
                table: "Branches",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "Branches",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Branches_Branches_Id",
                table: "Product_Branches",
                column: "Id",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Branches_Branches_Id",
                table: "Product_Branches");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Product_Branches",
                newName: "BranchID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Producers",
                newName: "ProducerID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Countries",
                newName: "CountryID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Branches",
                newName: "BranchID");

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Branches_Branches_BranchID",
                table: "Product_Branches",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
