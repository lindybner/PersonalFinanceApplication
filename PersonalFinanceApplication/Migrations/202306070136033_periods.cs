namespace PersonalFinanceApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class periods : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        PeriodId = c.Int(nullable: false, identity: true),
                        PeriodYear = c.Int(nullable: false),
                        PeriodMonth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PeriodId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Periods");
        }
    }
}
