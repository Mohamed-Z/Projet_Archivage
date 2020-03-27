namespace Projet1_ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encadrants", "confirmation", c => c.String());
            AlterColumn("dbo.Encadrants", "nom", c => c.String(nullable: false));
            AlterColumn("dbo.Encadrants", "prenom", c => c.String(nullable: false));
            AlterColumn("dbo.Encadrants", "password", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Encadrants", "password", c => c.String());
            AlterColumn("dbo.Encadrants", "prenom", c => c.String());
            AlterColumn("dbo.Encadrants", "nom", c => c.String());
            DropColumn("dbo.Encadrants", "confirmation");
        }
    }
}
