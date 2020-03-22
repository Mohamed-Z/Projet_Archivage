namespace Projet1_ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Encadrants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nom = c.String(),
                        prenom = c.String(),
                        email = c.String(),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groupes",
                c => new
                    {
                        grp_id = c.Int(nullable: false, identity: true),
                        type = c.String(),
                        id_enc = c.Int(),
                    })
                .PrimaryKey(t => t.grp_id)
                .ForeignKey("dbo.Encadrants", t => t.id_enc)
                .Index(t => t.id_enc);
            
            CreateTable(
                "dbo.Etudiants",
                c => new
                    {
                        cne = c.Int(nullable: false, identity: true),
                        nom = c.String(),
                        prenom = c.String(),
                        date_naiss = c.String(),
                        email = c.String(),
                        password = c.String(),
                        id_fil = c.Int(),
                        id_grp = c.Int(),
                        photo = c.Binary(),
                    })
                .PrimaryKey(t => t.cne)
                .ForeignKey("dbo.Filieres", t => t.id_fil)
                .ForeignKey("dbo.Groupes", t => t.id_grp)
                .Index(t => t.id_fil)
                .Index(t => t.id_grp);
            
            CreateTable(
                "dbo.Filieres",
                c => new
                    {
                        Id_filiere = c.Int(nullable: false, identity: true),
                        Nom_filiere = c.String(),
                    })
                .PrimaryKey(t => t.Id_filiere);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        groupe_Id = c.Int(nullable: false),
                        Name = c.String(),
                        Type = c.String(),
                        Length = c.Int(nullable: false),
                        Content = c.Binary(),
                        sujet = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.groupe_Id)
                .ForeignKey("dbo.Groupes", t => t.groupe_Id)
                .Index(t => t.groupe_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "groupe_Id", "dbo.Groupes");
            DropForeignKey("dbo.Etudiants", "id_grp", "dbo.Groupes");
            DropForeignKey("dbo.Etudiants", "id_fil", "dbo.Filieres");
            DropForeignKey("dbo.Groupes", "id_enc", "dbo.Encadrants");
            DropIndex("dbo.Files", new[] { "groupe_Id" });
            DropIndex("dbo.Etudiants", new[] { "id_grp" });
            DropIndex("dbo.Etudiants", new[] { "id_fil" });
            DropIndex("dbo.Groupes", new[] { "id_enc" });
            DropTable("dbo.Files");
            DropTable("dbo.Filieres");
            DropTable("dbo.Etudiants");
            DropTable("dbo.Groupes");
            DropTable("dbo.Encadrants");
        }
    }
}
