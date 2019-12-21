namespace RatesStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Type = c.Int(nullable: false),
                        LogTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.RateRelations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ExpiresAt = c.DateTime(nullable: false),
                        RateFrom_ID = c.Int(),
                        RateTo_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Rates", t => t.RateFrom_ID)
                .ForeignKey("dbo.Rates", t => t.RateTo_ID)
                .Index(t => t.RateFrom_ID)
                .Index(t => t.RateTo_ID);
            
            CreateTable(
                "dbo.RateRequestHistories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        To = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RateRelations", "RateTo_ID", "dbo.Rates");
            DropForeignKey("dbo.RateRelations", "RateFrom_ID", "dbo.Rates");
            DropIndex("dbo.RateRelations", new[] { "RateTo_ID" });
            DropIndex("dbo.RateRelations", new[] { "RateFrom_ID" });
            DropIndex("dbo.Rates", new[] { "Name" });
            DropTable("dbo.RateRequestHistories");
            DropTable("dbo.RateRelations");
            DropTable("dbo.Rates");
            DropTable("dbo.Logs");
        }
    }
}
