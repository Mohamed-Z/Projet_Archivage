namespace Projet1_ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cycles",
                c => new
                    {
                        id_Cycle = c.Int(nullable: false, identity: true),
                        nom_Cycle = c.String(),
                    })
                .PrimaryKey(t => t.id_Cycle);
            
            CreateTable(
                "dbo.Niveaux",
                c => new
                    {
                        id_Niveau = c.Int(nullable: false, identity: true),
                        nom_Niveau = c.String(),
                    })
                .PrimaryKey(t => t.id_Niveau);
            
            AddColumn("dbo.Etudiants", "id_cyc", c => c.Int());
            AddColumn("dbo.Etudiants", "id_niv", c => c.Int());
            CreateIndex("dbo.Etudiants", "id_cyc");
            CreateIndex("dbo.Etudiants", "id_niv");
            AddForeignKey("dbo.Etudiants", "id_cyc", "dbo.Cycles", "id_Cycle");
            AddForeignKey("dbo.Etudiants", "id_niv", "dbo.Niveaux", "id_Niveau");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Etudiants", "id_niv", "dbo.Niveaux");
            DropForeignKey("dbo.Etudiants", "id_cyc", "dbo.Cycles");
            DropIndex("dbo.Etudiants", new[] { "id_niv" });
            DropIndex("dbo.Etudiants", new[] { "id_cyc" });
            DropColumn("dbo.Etudiants", "id_niv");
            DropColumn("dbo.Etudiants", "id_cyc");
            DropTable("dbo.Niveaux");
            DropTable("dbo.Cycles");
        }
    }
}
