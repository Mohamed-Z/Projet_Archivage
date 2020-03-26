using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class SiteContext : DbContext
    {
        public SiteContext() : base("name=SiteContext")
        {

        }

        public DbSet<Encadrant> encadrants { get; set; }
        public DbSet<Etudiant> etudiants { get; set; }
        public DbSet<File> files { get; set; }
        public DbSet<Filiere> filieres { get; set; }
        public DbSet<Groupe> groupes { get; set; }
        public DbSet<Cycle> cycles { get; set; }
        public DbSet<Niveau> niveaux { get; set; }
        public DbSet<GroupeMembre> GroupeMembres { get; set; }
    }
}