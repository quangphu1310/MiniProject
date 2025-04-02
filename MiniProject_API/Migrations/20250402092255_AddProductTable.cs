using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniProject_API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Fantasy" },
                    { 5, "Mystery" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "H.G. Wells", 2, "A SciFi classic", "978-1-234567-01-1", 299.99000000000001, "The War of Worlds" },
                    { 3, "Stephen Hawking", 3, "Understanding the universe", "978-1-234567-03-3", 349.99000000000001, "A Brief History of Time" },
                    { 4, "Frank Herbert", 2, "An epic SciFi saga", "978-1-234567-04-4", 399.99000000000001, "Dune" },
                    { 8, "Orson Scott Card", 2, "A SciFi military story", "978-1-234567-08-8", 259.99000000000001, "Ender's Game" },
                    { 9, "Sun Tzu", 3, "A military strategy book", "978-1-234567-09-9", 149.99000000000001, "The Art of War" },
                    { 10, "George Orwell", 3, "A dystopian classic", "978-1-234567-10-0", 189.99000000000001, "1984" },
                    { 2, "Dan Brown", 5, "A thrilling mystery novel", "978-1-234567-02-2", 199.99000000000001, "The Lost Symbol" },
                    { 5, "J.K. Rowling", 4, "A young wizard's journey", "978-1-234567-05-5", 299.99000000000001, "Harry Potter and the Sorcerer's Stone" },
                    { 6, "J.R.R. Tolkien", 4, "A fantasy adventure", "978-1-234567-06-6", 279.99000000000001, "The Hobbit" },
                    { 7, "Dan Brown", 5, "A historical mystery", "978-1-234567-07-7", 219.99000000000001, "The Da Vinci Code" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
