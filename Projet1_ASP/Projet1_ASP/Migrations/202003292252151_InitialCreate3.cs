namespace Projet1_ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupeMembres", "confirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupeMembres", "token", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupeMembres", "token");
            DropColumn("dbo.GroupeMembres", "confirmed");
        }
    }
}
