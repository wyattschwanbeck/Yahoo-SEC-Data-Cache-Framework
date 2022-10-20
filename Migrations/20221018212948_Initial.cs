using Microsoft.EntityFrameworkCore.Migrations;

namespace EdgarCacheFramework.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialStatementsInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PullTime = table.Column<long>(nullable: false),
                    Ticker = table.Column<string>(nullable: true),
                    PeriodStart = table.Column<long>(nullable: false),
                    PeriodEnd = table.Column<long>(nullable: false),
                    Revenue = table.Column<float>(nullable: true),
                    SellingGeneralAndAdministrativeExpense = table.Column<float>(nullable: true),
                    ResearchAndDevelopmentExpense = table.Column<float>(nullable: true),
                    OperatingIncome = table.Column<float>(nullable: true),
                    NetIncome = table.Column<float>(nullable: true),
                    Assets = table.Column<float>(nullable: true),
                    Liabilities = table.Column<float>(nullable: true),
                    Equity = table.Column<float>(nullable: true),
                    Cash = table.Column<float>(nullable: true),
                    CurrentAssets = table.Column<float>(nullable: true),
                    CurrentLiabilities = table.Column<float>(nullable: true),
                    RetainedEarnings = table.Column<float>(nullable: true),
                    CommonStockSharesOutstanding = table.Column<long>(nullable: true),
                    OperatingCashFlows = table.Column<float>(nullable: true),
                    InvestingCashFlows = table.Column<float>(nullable: true),
                    FinancingCashFlows = table.Column<float>(nullable: true),
                    ProceedsFromIssuanceOfDebt = table.Column<float>(nullable: true),
                    PaymentsOfDebt = table.Column<float>(nullable: true),
                    DividendsPaid = table.Column<float>(nullable: true),
                    xml = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialStatementsInstances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialStatementsInstances");
        }
    }
}
