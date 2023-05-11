using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manga.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tryToaddPaymentDateToOrderHeader4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<DateTime>(
				name: "PaymentDate",
				table: "OrderHeaders",
				nullable: false,
				type: "datetime2");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
