namespace Projet1_ASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                "dbo.Etudiants",
                c => new
                    {
                        cne = c.Int(nullable: false),
                        nom = c.String(nullable: false),
                        prenom = c.String(nullable: false),
                        date_naiss = c.String(nullable: false),
                        email = c.String(nullable: false),
                        password = c.String(nullable: false, maxLength: 20),
                        confirmation = c.String(),
                        id_fil = c.Int(nullable: false),
                        id_cyc = c.Int(nullable: false),
                        id_niv = c.Int(nullable: false),
                        photo = c.Binary(),
                    })
                .PrimaryKey(t => t.cne)
                .ForeignKey("dbo.Cycles", t => t.id_cyc, cascadeDelete: true)
                .ForeignKey("dbo.Filieres", t => t.id_fil, cascadeDelete: true)
                .ForeignKey("dbo.Niveaux", t => t.id_niv, cascadeDelete: true)
                .Index(t => t.id_fil)
                .Index(t => t.id_cyc)
                .Index(t => t.id_niv);
            
            CreateTable(
                "dbo.Filieres",
                c => new
                    {
                        Id_filiere = c.Int(nullable: false, identity: true),
                        Nom_filiere = c.String(),
                    })
                .PrimaryKey(t => t.Id_filiere);
            
            CreateTable(
                "dbo.GroupeMembres",
                c => new
                    {
                        id_gm = c.Int(nullable: false, identity: true),
                        id_grp = c.Int(),
                        id_et = c.Int(),
                        date = c.String(),
                        confirmed = c.Boolean(nullable: false),
                        token = c.String(),
                    })
                .PrimaryKey(t => t.id_gm)
                .ForeignKey("dbo.Etudiants", t => t.id_et)
                .ForeignKey("dbo.Groupes", t => t.id_grp)
                .Index(t => t.id_grp)
                .Index(t => t.id_et);
            
            CreateTable(
                "dbo.Groupes",
                c => new
                    {
                        grp_id = c.Int(nullable: false, identity: true),
                        id_enc = c.Int(),
                        id_tp = c.Int(),
                    })
                .PrimaryKey(t => t.grp_id)
                .ForeignKey("dbo.Encadrants", t => t.id_enc)
                .ForeignKey("dbo.Types", t => t.id_tp)
                .Index(t => t.id_enc)
                .Index(t => t.id_tp);
            
            CreateTable(
                "dbo.Encadrants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false),
                        prenom = c.String(nullable: false),
                        email = c.String(),
                        password = c.String(nullable: false, maxLength: 20),
                        confirmation = c.String(),
                        nbr_grp = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        groupe_Id = c.Int(nullable: false),
                        Name = c.String(),
                        Type = c.String(nullable: false),
                        Length = c.Int(nullable: false),
                        Content = c.Binary(),
                        sujet = c.String(nullable: false),
                        description = c.String(nullable: false),
                        date_disp = c.String(),
                    })
                .PrimaryKey(t => t.groupe_Id)
                .ForeignKey("dbo.Groupes", t => t.groupe_Id)
                .Index(t => t.groupe_Id);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        id_type = c.Int(nullable: false, identity: true),
                        nom_type = c.String(),
                    })
                .PrimaryKey(t => t.id_type);
            
            CreateTable(
                "dbo.Niveaux",
                c => new
                    {
                        id_Niveau = c.Int(nullable: false, identity: true),
                        nom_Niveau = c.String(),
                        code_cyc = c.Int(),
                    })
                .PrimaryKey(t => t.id_Niveau)
                .ForeignKey("dbo.Cycles", t => t.code_cyc)
                .Index(t => t.code_cyc);
            
            CreateTable(
                "dbo.SuperUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false),
                        prenom = c.String(nullable: false),
                        email = c.String(),
                        password = c.String(nullable: false, maxLength: 20),
                        confirmation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Etudiants", "id_niv", "dbo.Niveaux");
            DropForeignKey("dbo.Niveaux", "code_cyc", "dbo.Cycles");
            DropForeignKey("dbo.GroupeMembres", "id_grp", "dbo.Groupes");
            DropForeignKey("dbo.Groupes", "id_tp", "dbo.Types");
            DropForeignKey("dbo.Files", "groupe_Id", "dbo.Groupes");
            DropForeignKey("dbo.Groupes", "id_enc", "dbo.Encadrants");
            DropForeignKey("dbo.GroupeMembres", "id_et", "dbo.Etudiants");
            DropForeignKey("dbo.Etudiants", "id_fil", "dbo.Filieres");
            DropForeignKey("dbo.Etudiants", "id_cyc", "dbo.Cycles");
            DropIndex("dbo.Niveaux", new[] { "code_cyc" });
            DropIndex("dbo.Files", new[] { "groupe_Id" });
            DropIndex("dbo.Groupes", new[] { "id_tp" });
            DropIndex("dbo.Groupes", new[] { "id_enc" });
            DropIndex("dbo.GroupeMembres", new[] { "id_et" });
            DropIndex("dbo.GroupeMembres", new[] { "id_grp" });
            DropIndex("dbo.Etudiants", new[] { "id_niv" });
            DropIndex("dbo.Etudiants", new[] { "id_cyc" });
            DropIndex("dbo.Etudiants", new[] { "id_fil" });
            DropTable("dbo.SuperUsers");
            DropTable("dbo.Niveaux");
            DropTable("dbo.Types");
            DropTable("dbo.Files");
            DropTable("dbo.Encadrants");
            DropTable("dbo.Groupes");
            DropTable("dbo.GroupeMembres");
            DropTable("dbo.Filieres");
            DropTable("dbo.Etudiants");
            DropTable("dbo.Cycles");
        }
    }
}
