namespace PRSClassesManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeallfieldsinTokentablerequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tokens", "User_Id", "dbo.Users");
            DropIndex("dbo.Tokens", new[] { "User_Id" });
            AlterColumn("dbo.Tokens", "Key", c => c.String(nullable: false));
            AlterColumn("dbo.Tokens", "User_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Tokens", "User_Id");
            AddForeignKey("dbo.Tokens", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tokens", "User_Id", "dbo.Users");
            DropIndex("dbo.Tokens", new[] { "User_Id" });
            AlterColumn("dbo.Tokens", "User_Id", c => c.Int());
            AlterColumn("dbo.Tokens", "Key", c => c.String());
            CreateIndex("dbo.Tokens", "User_Id");
            AddForeignKey("dbo.Tokens", "User_Id", "dbo.Users", "Id");
        }
    }
}
