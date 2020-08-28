using Microsoft.EntityFrameworkCore.Migrations;

namespace TestCaseLegiosoft.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionModels",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionStatus = table.Column<string>(nullable: false),
                    TransactionType = table.Column<string>(nullable: false),
                    ClientName = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(13, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionModels", x => x.TransactionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionModels");
        }
    }
}
