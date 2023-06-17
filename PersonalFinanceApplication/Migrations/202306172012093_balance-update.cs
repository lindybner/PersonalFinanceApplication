namespace PersonalFinanceApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class balanceupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Balances", "ChequingAcct", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Balances", "SavingsAcct", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Balances", "RevolvingCrdt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Balances", "InstalmentCrdt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Balances", "OwnBalance");
            DropColumn("dbo.Balances", "OweBalance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Balances", "OweBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Balances", "OwnBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Balances", "InstalmentCrdt");
            DropColumn("dbo.Balances", "RevolvingCrdt");
            DropColumn("dbo.Balances", "SavingsAcct");
            DropColumn("dbo.Balances", "ChequingAcct");
        }
    }
}
