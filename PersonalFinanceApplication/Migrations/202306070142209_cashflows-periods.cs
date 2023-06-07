namespace PersonalFinanceApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cashflowsperiods : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cashflows",
                c => new
                    {
                        CashflowId = c.Int(nullable: false, identity: true),
                        PeriodId = c.Int(nullable: false),
                        Inflow = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Outflow = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CashflowId)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => t.PeriodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cashflows", "PeriodId", "dbo.Periods");
            DropIndex("dbo.Cashflows", new[] { "PeriodId" });
            DropTable("dbo.Cashflows");
        }
    }
}
