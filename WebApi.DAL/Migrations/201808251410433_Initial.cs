namespace WebApi.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                        Visits = c.Int(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Index);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Requests");
        }
    }
}
