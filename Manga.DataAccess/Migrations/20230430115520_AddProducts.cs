using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Manga.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProducts : Migration
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
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VolumeNumber = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "Price", "Title", "VolumeNumber" },
                values: new object[,]
                {
                    { 1, "Gamon Sakurai", "A bright high schooler has discovered to his horror that death is just a repeatabl event for him—and that humanity has no mercy for a demi-human. To avoid becoming a science experiment for the rest of his interminable life, Kei Nagai must seek out others of his kind. But what would a community of them stand for?", "9781939130846", 67.0, "Ajin Demi-Human", 1 },
                    { 2, "Gamon Sakurai", "A bright high schooler has discovered to his horror that death is just a repeatabl event for him—and that humanity has no mercy for a demi-human. To avoid becoming a science experiment for the rest of his interminable life, Kei Nagai must seek out others of his kind. But what would a community of them stand for?", "9781939130853", 67.0, "Ajin Demi-Human", 2 },
                    { 3, "Hajime Isayama", "Several hundred years ago, humans were nearly exterminated by giants. Giants are typically several stories tall, seem to have no intelligence and who devour human beings.\r\n\r\nA small percentage of humanity survied barricading themselves in a city protected by walls even taller than the biggest of giants.\r\n\r\nFlash forward to the present and the city has not seen a giant in over 100 years - before teenager Elen and his foster sister Mikasa witness something horrific as the city walls are destroyed by a super-giant that appears from nowhere.\r\n\r\n", "9781612620244", 60.0, "Attack on Titan", 1 },
                    { 4, "Takeshi Obata", "Light tests the boundaries of the Death Note's powers as L and the police begin to close in. Luckily Light's father is the head of the Japanese National Police Agency and leaves vital information about the case lying around the house. With access to his father's files, Light can keep one step ahead of the authorities.\r\n\r\nBut who is the strange man following him, and how can Light guard against enemies whose names he doesn't know?", "9781421501680", 52.0, "Death Note", 1 },
                    { 5, "Makoto Yukimura", "As a child, Thorfinn sat at the feet of the great Leif Ericson and thrilled to wild tales of a land far to the west, but his youthful fantasies were shattered by a mercenary raid. Raised by the Vikings who murdered his family, Thorfinn became a terrifying warrior, forever seeking to kill the band's leader, Askeladd, and avenge his father.\r\n\r\nSustaining Thorfinn through his ordeal are his pride in his family and his dreams of a fertile westward land, a land without war or slavery... the land Leif called Vinland.", "9781612624204", 52.0, "Vinland Saga", 1 },
                    { 6, "Kohei Horikoshi", "What would the world be like if 80 percent of the population manifested superpowers called “Quirks” at age four? Heroes and villains would be battling it out everywhere! Being a hero would mean learning to use your power, but where would you go to study? The Hero Academy of course! But what would you do if you were one of the 20 percent who were born Quirkless?\r\n\r\nMiddle school student Izuku Midoriya wants to be a hero more than anything, but he hasn’t got an ounce of power in him. With no chance of ever getting into the prestigious U.A. High School for budding heroes, his life is looking more and more like a dead end. Then an encounter with All Might, the greatest hero of them all, gives him a chance to change his destiny…", "9781421582696", 118.0, "My Hero Academia", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
