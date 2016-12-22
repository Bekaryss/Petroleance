namespace Petroleance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstAnswerr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        TestId = c.Int(nullable: false),
                        CorrectAnswer = c.Int(nullable: false),
                        WrongAnswer = c.Int(nullable: false),
                        Percentage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TriviaAnswers", "TestId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TriviaAnswers", "TestId");
            DropTable("dbo.Statistics");
        }
    }
}
