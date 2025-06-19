namespace todolist_mvvm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompletionFieldsToTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "IsCompleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "CompletedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "CompletedAt");
            DropColumn("dbo.Tasks", "IsCompleted");
        }
    }
}
