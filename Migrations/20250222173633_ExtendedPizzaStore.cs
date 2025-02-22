using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoPizza.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedPizzaStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Pizzas_PizzaId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Pizzas");

            migrationBuilder.RenameColumn(
                name: "PizzaId",
                table: "OrderItems",
                newName: "PizzaSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_PizzaId",
                table: "OrderItems",
                newName: "IX_OrderItems_PizzaSizeId");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Pizzas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "PizzaSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaSizes_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PizzaSizes_PizzaId_Size",
                table: "PizzaSizes",
                columns: new[] { "PizzaId", "Size" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_PizzaSizes_PizzaSizeId",
                table: "OrderItems",
                column: "PizzaSizeId",
                principalTable: "PizzaSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_PizzaSizes_PizzaSizeId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "PizzaSizes");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "PizzaSizeId",
                table: "OrderItems",
                newName: "PizzaId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_PizzaSizeId",
                table: "OrderItems",
                newName: "IX_OrderItems_PizzaId");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Pizzas",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Pizzas_PizzaId",
                table: "OrderItems",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
