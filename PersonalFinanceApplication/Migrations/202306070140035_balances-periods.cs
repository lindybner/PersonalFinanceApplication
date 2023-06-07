namespace PersonalFinanceApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class balancesperiods : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Balances",
                c => new
                    {
                        BalanceId = c.Int(nullable: false, identity: true),
                        PeriodId = c.Int(nullable: false),
                        OwnBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OweBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.BalanceId)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => t.PeriodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Balances", "PeriodId", "dbo.Periods");
            DropIndex("dbo.Balances", new[] { "PeriodId" });
            DropTable("dbo.Balances");
        }
    }
}
