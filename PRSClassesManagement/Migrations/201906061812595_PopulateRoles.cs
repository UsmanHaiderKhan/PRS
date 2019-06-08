namespace PRSClassesManagement.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PopulateRoles : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Roles (Name, Rank) VALUES ('admin', 1)");
            Sql("INSERT INTO Roles (Name, Rank) VALUES ('user', 2)");
        }
        
        public override void Down()
        {
        }
    }
}
