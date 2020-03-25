namespace Projet1_ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Etudiants", "id_grp", "dbo.Groupes");
            DropIndex("dbo.Etudiants", new[] { "id_grp" });
            DropPrimaryKey("dbo.Etudiants");
            CreateTable(
                "dbo.GroupeMembres",
                c => new
                    {
                        id_gm = c.Int(nullable: false, identity: true),
                        id_grp = c.Int(),
                        id_et = c.Int(),
                        date = c.String(),
                    })
                .PrimaryKey(t => t.id_gm)
                .ForeignKey("dbo.Etudiants", t => t.id_et)
                .ForeignKey("dbo.Groupes", t => t.id_grp)
                .Index(t => t.id_grp)
                .Index(t => t.id_et);
            
            AddColumn("dbo.Encadrants", "nbr_grp", c => c.Int(nullable: false));
            AddColumn("dbo.Niveaux", "code_cyc", c => c.Int());
            AlterColumn("dbo.Etudiants", "cne", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Etudiants", "cne");
            CreateIndex("dbo.Niveaux", "code_cyc");
            AddForeignKey("dbo.Niveaux", "code_cyc", "dbo.Cycles", "id_Cycle");
            DropColumn("dbo.Etudiants", "id_grp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Etudiants", "id_grp", c => c.Int());
            DropForeignKey("dbo.Niveaux", "code_cyc", "dbo.Cycles");
            DropForeignKey("dbo.GroupeMembres", "id_grp", "dbo.Groupes");
            DropForeignKey("dbo.GroupeMembres", "id_et", "dbo.Etudiants");
            DropIndex("dbo.Niveaux", new[] { "code_cyc" });
            DropIndex("dbo.GroupeMembres", new[] { "id_et" });
            DropIndex("dbo.GroupeMembres", new[] { "id_grp" });
            DropPrimaryKey("dbo.Etudiants");
            AlterColumn("dbo.Etudiants", "cne", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Niveaux", "code_cyc");
            DropColumn("dbo.Encadrants", "nbr_grp");
            DropTable("dbo.GroupeMembres");
            AddPrimaryKey("dbo.Etudiants", "cne");
            CreateIndex("dbo.Etudiants", "id_grp");
            AddForeignKey("dbo.Etudiants", "id_grp", "dbo.Groupes", "grp_id");
        }
    }
}
