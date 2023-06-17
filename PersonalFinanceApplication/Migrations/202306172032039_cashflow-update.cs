namespace PersonalFinanceApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cashflowupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cashflows", "PrimaryIncome", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Cashflows", "MiscIncome", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Cashflows", "NecessarySpend", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Cashflows", "DiscretionarySpend", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Cashflows", "Inflow");
            DropColumn("dbo.Cashflows", "Outflow");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cashflows", "Outflow", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Cashflows", "Inflow", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Cashflows", "DiscretionarySpend");
            DropColumn("dbo.Cashflows", "NecessarySpend");
            DropColumn("dbo.Cashflows", "MiscIncome");
            DropColumn("dbo.Cashflows", "PrimaryIncome");
        }
    }
}
