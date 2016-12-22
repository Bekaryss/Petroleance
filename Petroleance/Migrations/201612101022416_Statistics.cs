namespace Petroleance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statistics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Statistics", "TriviaTests_Id", c => c.Int());
            CreateIndex("dbo.Statistics", "TriviaTests_Id");
            AddForeignKey("dbo.Statistics", "TriviaTests_Id", "dbo.TriviaTests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Statistics", "TriviaTests_Id", "dbo.TriviaTests");
            DropIndex("dbo.Statistics", new[] { "TriviaTests_Id" });
            DropColumn("dbo.Statistics", "TriviaTests_Id");
        }
    }
}
