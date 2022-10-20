using Microsoft.EntityFrameworkCore.Migrations;

namespace EdgarCacheFramework.Migrations
{
    public partial class StockPriceAndEdgarCaching : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockPriceInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ticker = table.Column<string>(nullable: true),
                    Date = table.Column<long>(nullable: false),
                    Open = table.Column<float>(nullable: false),
                    High = table.Column<float>(nullable: false),
                    Low = table.Column<float>(nullable: false),
                    Close = table.Column<float>(nullable: false),
                    AdjustedClose = table.Column<float>(nullable: false),
                    Volume = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPriceInstances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockPriceInstances");
        }
    }
}
